using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {
    public GameObject lazerPrefab;



    // configuration parameters
    [Header("Player")]
    [SerializeField] float speed = 10f;
    [SerializeField] float playerPadding = .5f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip playerFiring;
    [SerializeField] AudioClip playerDying;
    [SerializeField] [Range(0, 1)] float firingSoundVolume = 0.25f;
    [SerializeField] [Range(0, 1)] float dyingSoundVolume = 0.25f;



    [Header("PRojectiles")]
    [SerializeField] float projectileSpeed = 10f;
  [SerializeField] float projectileFiringPeriod = .01f;
    Coroutine firingCoroutine;


    float xMin;
    float yMin;
    float xMax;
    float yMax;

    



    // Use this for initialization
    void Start () {

        SetUpBoundries();


	}

   

    // Update is called once per frame
    void Update () {

        Move();
        Fire();

        
	}


    private void Move()
    {
       
        var deltaX = Input.GetAxis("Horizontal");
        var deltaY = Input.GetAxis("Vertical");

        var newXPos = Mathf.Clamp( transform.position.x+ deltaX  * speed* Time.deltaTime,xMin,xMax);
        var newYPos = Mathf.Clamp( transform.position.y + deltaY  * speed* Time.deltaTime, yMin,yMax);

        transform.position = new Vector2(newXPos , newYPos);


    }


    private void Fire()
    {
        

       if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine= StartCoroutine( FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }


    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(lazerPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerFiring, Camera.main.transform.position,firingSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }


       
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer > ();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
        
        
    }

    public void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health<= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        AudioSource.PlayClipAtPoint(playerDying, Camera.main.transform.position, dyingSoundVolume);
        Destroy(gameObject);
    }

    private void SetUpBoundries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + playerPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + playerPadding;

        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + -playerPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + -playerPadding;
    }

}
 