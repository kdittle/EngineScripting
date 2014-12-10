
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidScript : MonoBehaviour
{
    public Transform explosion;

    public GameObject[] childAsteroids;
    public int numChildren = 2;

    public float minTorque = 10.0f;
    public float maxTorque = 50.0f;

    public float minForce = 10.0f;
    public float maxForce = 15.0f;

    private Transform tempObject;

    private Vector2 velocity;
    private float x, y;
    private float magnitude;

    // Use this for initialization
    void Start()
    {
        //randomly start moving asteroids at the start
        magnitude = Random.Range(minForce, maxForce);

        x = Random.Range(-1.0f, 1.0f);
        y = Random.Range(-1.0f, 1.0f);

        rigidbody2D.AddForce(new Vector2(x, y) * magnitude);


        rigidbody2D.AddTorque(Random.Range(minTorque, maxTorque));

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "bullet")
        {
            //find the game manager and send it a message to update the player score
            GameObject.FindGameObjectWithTag("Game Manager").SendMessage("UpdatePlayerScore", 100);

            //create the explosion
            Instantiate(explosion, transform.position, transform.rotation);

            //destroy game objects
            Destroy(otherObject.gameObject);
            Destroy(gameObject);


            GameObject.FindGameObjectWithTag("Game Manager").SendMessage("UpdateAsteroidCount", -1);

            //split the parent asteroid
            if (childAsteroids[0] != null)
            {
                //send a message to add 2 asteroids to the count
                GameObject.FindGameObjectWithTag("Game Manager").SendMessage("UpdateAsteroidCount", 2);
                for (int i = 0; i < numChildren; i++)
                {
                    int r = Random.Range(-3, 3);
                    Instantiate(childAsteroids[Random.Range(0, childAsteroids.Length)], transform.position + new Vector3(r, r, 0), new Quaternion());

                }
            }

        }

        //destroy asteroids if they collide with alien bullets or a UFO
        if (otherObject.gameObject.tag == "alienBullet" || otherObject.gameObject.tag == "enemy")
        {
            //create explosion
            Instantiate(explosion, transform.position, transform.rotation);

            //destory game objects
            Destroy(otherObject.gameObject);
            Destroy(gameObject);

            //create children asteroids (split the initial asteroid)
            if (childAsteroids[0] != null)
            {
                for (int i = 0; i < numChildren; i++)
                {
                    int r = Random.Range(-3, 3);
                    Instantiate(childAsteroids[Random.Range(0, childAsteroids.Length)], transform.position + new Vector3(r, r, 0), new Quaternion());
                }
            }
        }

    }
}
