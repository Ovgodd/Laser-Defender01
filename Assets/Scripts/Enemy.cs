using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Ennemie")]
	[SerializeField] float health = 100;
	float shotCounter;
	[SerializeField] int scoreValue = 150;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;

	[Header("Projectile")]
	[SerializeField] GameObject enemieMissile;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] float projectileFiringPeriod = 0.1f;

	[Header("Effects")]
	[SerializeField] GameObject deathVFX;
	[SerializeField] float durationOfExplosion = 1f;
	[SerializeField] AudioClip explosionSound;
	[SerializeField] AudioClip projectileSound;
	[SerializeField] [Range(0,1)] float explosionSoundVolume = 0.7f;
	[SerializeField] [Range(0,1)] float projectileSoundVolume = 0.4f;

	// Start is called before the first frame update
	void Start()
	{
		shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}

	// Update is called once per frame
	void Update()
	{
		CountDownAndShoot();
	}

	private void CountDownAndShoot()
	{
		shotCounter -= Time.deltaTime;
		if (shotCounter <= 0f)
		{
			Fire();
			shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
			
		}
	}
	private void Fire()
	{
		GameObject laser = Instantiate(enemieMissile, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
		AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileSoundVolume);
		transform.rotation = Quaternion.Euler(180, 0, 0);


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
			PlayEnemyDestroySFX();
		}
	}
	private void PlayEnemyDestroySFX()
	{
		AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionSoundVolume) ;
		Destroy(gameObject);
		Die();
	}
	private void Die()
	{
		FindObjectOfType<GameSession>().AddToScore(scoreValue);
		Destroy(gameObject);
		GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
		Destroy(explosion, durationOfExplosion);
	}




}
