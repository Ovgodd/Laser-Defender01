using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
	// configuration parameters
	[Header("Player")]
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float Xpadding = 1f;
	[SerializeField] float Ypadding = 1f;
	[SerializeField] int health = 300;
	
	

	[Header("Projectile")]
	[SerializeField] GameObject laserPrefab;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] float projectileFiringPeriod = 0.1f;

	[Header("Effects")]
	[SerializeField] GameObject deathVFX;
	[SerializeField] float durationOfExplosion = 1f;
	[SerializeField] AudioClip explosionSound;

	[SerializeField] AudioClip machineGun;
	[SerializeField] [Range(0,1)] float explosionSoundVolume;
	[SerializeField] [Range(0,1)] float projectileSoundVolume;

	Coroutine firingCoroutine;
	Coroutine soundCoroutine;

	Level level;
	float xMin;
	float xMax;
	float yMin;
	float yMax;
	// Start is called before the first frame update
	void Start()
	{
		SetUpMoveBoundaries();

	}

	

	// Update is called once per frame
	void Update()
	{
		Move();
		Fire();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageDeal damageDeal = other.gameObject.GetComponent<DamageDeal>();
		if (!damageDeal) { return; }
		ProcessHit(damageDeal);
	}
	private void ProcessHit(DamageDeal damageDeal)
	{
		health -= damageDeal.GetDamage();
		damageDeal.Hit();
		if (health <= 0)
		{
			Die();
			PlayPlayerDestroySFX();
			
		}
	}

		
	private void Die()
	
	{
		
		FindObjectOfType<Level>().LoadGameOver();
		Destroy(gameObject);
		GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
		
	}
	private void PlayPlayerDestroySFX()
	{
		AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionSoundVolume);
		Destroy(gameObject);
		Die();
		
	}
	public void Fire()
	{
	
		if (Input.GetButtonDown("Fire1"))
		{
			firingCoroutine = StartCoroutine(FireContinously());

		}
		if (Input.GetButtonUp("Fire1"))
		{
			StopCoroutine(firingCoroutine);

		}
	}


	IEnumerator FireContinously()
	{

		while (true)
		{
			GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
			AudioSource.PlayClipAtPoint(machineGun, Camera.main.transform.position, projectileSoundVolume);
			yield return new WaitForSeconds(projectileFiringPeriod);
		}

	}
	public int GetHealth()
	{
		return health;
	}

	private void Move()
	{
		var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		var neWPosX = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
		var neWPosY = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
		transform.position = new Vector2(neWPosX, neWPosY);

	}


	private void SetUpMoveBoundaries()
	{
		Camera gameCamera = Camera.main;
		xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + Xpadding;
		xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - Xpadding;
		yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + Ypadding;
		yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - Ypadding;
	}


}
