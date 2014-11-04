using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidScript : MonoBehaviour
{
    public Transform explosion;

    public GameObject[] AsteroidObjects = new GameObject[3]; 

    private Vector2 spawnPosition;
    private Vector2 velocity;

    private float minScale = 1.0f;
    private float maxScale = 3.0f;

    private float numAsteroids = 0;
    private float maxNumAsteroids = 10;
    private float maxNumAsteroidsOnScreen = 4;

	// Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    
	}

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "bullet")
        {
            PlayerScript.playerScore += 100;

            Transform tempExplosion;

            tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(otherObject.gameObject);
            Destroy(gameObject);

        }
    }
}
