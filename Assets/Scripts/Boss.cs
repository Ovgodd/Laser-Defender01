using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	[Header("Boss")]
	[SerializeField] float bossHealth = 15000;
	[SerializeField] float shotCounter;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[SerializeField] int scoreValue = 500;

	[Header("Projectile")]
	[SerializeField] GameObject enemieMissile;
	[SerializeField] float projectileSpeed = 15f;
	[SerializeField] float projectileFiringPeriod = 0.1f;

	[Header("Effects")]
	[SerializeField] GameObject deathVFX;
	[SerializeField] float durationOfExplosion = 1f;
	[SerializeField] AudioClip explosionSound;
	[SerializeField] AudioClip projectileSound;
	[SerializeField] [Range(0, 1)] float explosionSoundVolume = 0.7f;
	[SerializeField] [Range(0, 1)] float projectileSoundVolume = 0.4f;
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
			AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, explosionSoundVolume);
		}
	}
	private void Fire()
	{
		GameObject laser = Instantiate(enemieMissile, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
		transform.rotation = Quaternion.Euler(180, 0, 0);


	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		DamageDeal damageDeal = other.gameObject.GetComponent<DamageDeal>();
		if (!damageDeal) { return; }
		ProcessBossHit(damageDeal);
	}

	private void ProcessBossHit(DamageDeal damageDeal)
	{
		bossHealth -= damageDeal.GetDamage();
		damageDeal.Hit();
		if (bossHealth <= 0)
		{
			Die();
			PlayEnemyDestroySFX();
		}
	}

	private void Die()
	{
		FindObjectOfType<GameSession>().AddToScore(scoreValue);
		Destroy(gameObject);
		GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
		Destroy(explosion, durationOfExplosion);		
	}
	private void PlayEnemyDestroySFX()
	{
		AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
		Destroy(gameObject);
		Die();
	}
}
