using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float health = 200;

    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float enemyLaserSpeed = 10f;

    [SerializeField] GameObject deathVFX;
    [SerializeField] float explotionDuration;

    //reduce health everytime an enemy collides with a gameObject
    //which has a DamageDealer component
    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        //access the DamageDealer class form "otherobject" which hits enemy
        //and reduce health accordingly
        DamageDealer dmg = otherObject.gameObject.GetComponent<DamageDealer>();

        //if there is no damage dealer in otherObject end the method.
        if (!dmg) //if (dmg == null).
        {
            return;
        }

        ProcessHit(dmg);
    }

    //whenever ProcessHit() is called, send us the DamageDealer details
    private void ProcessHit(DamageDealer dmg)
    {
        health -= dmg.GetDamage();

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);

        //create an Explosion Particle
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        //destroy explosion after explosionDuration
        Destroy(explosion, explotionDuration);
    }

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

    //count down shotCounter to 0 and shoot
    private void CountDownAndShoot()
    {
        //every frame reduce the amount of time of shotCounter
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0f)
        {
            EnemyFire();
            //reset shotCounter after every fire
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    //spawn an enemy laser from the enemy's position
    private void EnemyFire()
    {
        
        GameObject enemyLaser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity);

        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);
    }
}
