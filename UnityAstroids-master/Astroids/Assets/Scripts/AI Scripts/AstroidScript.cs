using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidScript : MonoBehaviour
{
    public Transform explosion;

    public GameObject[] AsteroidObjects = new GameObject[3];

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
            PlayerScript.playerScore += 100;

            Transform tempExplosion;

            tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(otherObject.gameObject);
            Destroy(gameObject);

            tempObject = Instantiate(transform, transform.position, transform.rotation) as Transform;
            tempObject.localScale = new Vector3(3.0f, 3.0f, 1.0f);
            tempObject.gameObject.collider2D.enabled = true;
            tempObject.rigidbody2D.AddForce(new Vector2(x, y) * magnitude);
            rigidbody2D.AddTorque(Random.Range(minTorque, maxTorque));

            tempObject = Instantiate(transform, transform.position + new Vector3(1, 1, 1), transform.rotation) as Transform;
            tempObject.localScale = new Vector3(3.0f, 3.0f, 1.0f);
            tempObject.gameObject.collider2D.enabled = true;
            tempObject.rigidbody2D.AddForce(new Vector2(x, y) * magnitude);
            rigidbody2D.AddTorque(Random.Range(minTorque, maxTorque));
        }
    }
}
