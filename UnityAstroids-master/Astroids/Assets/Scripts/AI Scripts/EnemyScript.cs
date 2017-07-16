using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour 
{
	public Transform target;
    public Rigidbody2D bullet;
    public Transform explosion;
    public float _timeToChangeDirection = 5.0f;
    public float _shootInterval = 2.0f;
    public float minForce = -100;
    public float maxForce = 100;

	// Use this for initialization
	void Start () 
	{
        Move();
        _timeToChangeDirection = 5.0f;
        _shootInterval = 2.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
        _timeToChangeDirection -= Time.deltaTime;
        if (_timeToChangeDirection <= 0)
        {
            Move();
            _timeToChangeDirection = 5.0f;
        }

        _shootInterval -= Time.deltaTime;
        if(Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= 35.0f 
            && _shootInterval <= 0)
        {
            ShootAtPlayer();
            _shootInterval = 2.0f;
        }
	}

    private void Move()
    {
        float _x = Random.Range(-1, 1);
        float _y = Random.Range(-1, 1);


        GetComponent<Rigidbody2D>().AddForce(new Vector2(_x, _y) * Random.Range(minForce, maxForce));
    }

    private void ShootAtPlayer()
    {
        float angle = (Mathf.Atan2(GameObject.FindGameObjectWithTag("Player").transform.position.y - transform.position.y,
            GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x) - Mathf.PI / 2 ) * Mathf.Rad2Deg;

        Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.position);

        Instantiate(bullet, transform.GetChild(0).transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "bullet")
        {
            GameObject.FindGameObjectWithTag("Game Manager").SendMessage("UpdatePlayerScore", 100);
            GameObject.FindGameObjectWithTag("Game Manager").SendMessage("RemoveUFO");

            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(otherObject.gameObject);
            Destroy(gameObject);
        }

        if (otherObject.gameObject.tag == "asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            GameObject.FindGameObjectWithTag("Game Manager").SendMessage("RemoveUFO");

            Destroy(gameObject);
        }
    }

}
