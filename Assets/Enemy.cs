using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
   

    [SerializeField] float health = 100f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBeforeShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion=1f;
    [SerializeField] AudioClip enemyFiring;
    [SerializeField] AudioClip enemyDying;
    [SerializeField] [Range(0,1)] float firingSoundVolume = 0.25f;
    [SerializeField] [Range(0,1)] float dyingSoundVolume = 0.25f;
    
    

	// Use this for initialization
	void Start () {
        shotCounter = Random.Range(minTimeBeforeShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update () {
        CountdownAndShoot();
	}

   public void CountdownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter<=0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBeforeShots, maxTimeBetweenShots);
        }
    }

    public void Fire()
    {
        
     enemyProjectile =  Instantiate(enemyProjectile, transform.position, Quaternion.identity)as GameObject;
        enemyProjectile.GetComponent<Rigidbody2D>().velocity= new Vector2 (0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(enemyFiring, Camera.main.transform.position, firingSoundVolume);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0f)
        {
            Die();
        }
        
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(enemyDying, Camera.main.transform.position, dyingSoundVolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity) as GameObject; 
        Destroy(gameObject);
        Destroy(explosion,durationOfExplosion);
        
    }
}
