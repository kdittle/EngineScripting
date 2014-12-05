
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidScript : MonoBehaviour
{
    public Transform explosion;

    public GameObject childAsteroid;
    public int numChildren = 2;

    public float minTorque = 10.0f;
    public float maxTorque = 50.0f;

    public float minForce = 10.0f;
    public float maxForce = 15.0f;

    private Transform tempObject;

    private Vector2 velocity;
    private float x, y;
    float magnitude;

    // Use this for initialization
    void Start()
    {
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
            GameObject.FindGameObjectWithTag("Game Manager").SendMessage("UpdatePlayerScore", 100);

            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(otherObject.gameObject);
            Destroy(gameObject);

            if (childAsteroid != null)
            {
                for (int i = 0; i < numChildren; i++)
                {
                    int r = Random.Range(-3, 3);
                    Instantiate(childAsteroid, transform.position + new Vector3(r, r, 0), new Quaternion());
                }
            }
        }

    }
}
