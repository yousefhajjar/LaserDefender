using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.7f;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserFiringSpeed = 0.3f;

    float xMin, xMax, yMin, yMax;

    Coroutine fireCoroutine;

    bool coroutineStarted;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        StartCoroutine(PrintAndWait());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    //coroutine to print 2 messages
    private IEnumerator PrintAndWait()
    {
        print("Message 1");
        yield return new WaitForSeconds(10);
        print("Message 2 after 10 seconds");
    }

    //coroutine to fire continuously

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15f);

            yield return new WaitForSeconds(laserFiringSpeed);
        }
    }

    private void Fire()
    {
        //if i press fire button
        if (Input.GetButtonDown("Fire1"))
        {
            if (!coroutineStarted) //if coroutineStarted == false
            {
                fireCoroutine = StartCoroutine(FireContinuously());
                coroutineStarted = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCoroutine);
            coroutineStarted = false;
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = transform.position.x + deltaX;

        newXPos = Mathf.Clamp(newXPos, xMin, xMax);

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newYPos = transform.position.y + deltaY;

        newYPos = Mathf.Clamp(newYPos, yMin, yMax);

        this.transform.position = new Vector2(newXPos, newYPos);

    }
}
