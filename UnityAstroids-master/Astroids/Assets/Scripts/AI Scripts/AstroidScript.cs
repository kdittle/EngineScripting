using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidScript : MonoBehaviour
{
    public Transform explosion;

    public GameObject[] AsteroidObjects = new GameObject[3];


    public GameObject gameManager;
    public GameManagerScript gmScript;

    private Vector2 spawnPosition;
    private Vector2 velocity;

    private float numAsteroids = 2;

    private Transform tempObject;

    void Awake()
    {
        try
        {
            gmScript = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (gmScript == null)
        {
            Debug.Log("There is no Game Manager");
            Debug.Log("Creating Game Manager");
            Instantiate(gameManager);

        }
        else
        {
            Debug.Log("Game Manager found.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "bullet")
        {
            PlayerScript.playerScore += 100;

            Transform tempExplosion;

            tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(otherObject.gameObject);
            Destroy(gameObject);

            tempObject = Instantiate(transform, transform.position, transform.rotation) as Transform;
            tempObject.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            tempObject.gameObject.collider2D.enabled = true;
            tempObject = Instantiate(transform, transform.position + new Vector3(1, 1, 1), transform.rotation) as Transform;
            tempObject.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            tempObject.gameObject.collider2D.enabled = true;
        }
    }
}
