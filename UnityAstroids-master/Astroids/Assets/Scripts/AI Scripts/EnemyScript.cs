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

    private GameObject playerRef;
    private PlayerScript playerRefScript;

	// Use this for initialization
	void Start () 
	{
        Move();
        _timeToChangeDirection = 5.0f;
        _shootInterval = 2.0f;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerRefScript = playerRef.GetComponent<PlayerScript>();
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
        if (playerRefScript.isPlayerAlive())
        {
            if (Vector2.Distance(playerRef.transform.position, transform.position) <= 35.0f
                && _shootInterval <= 0)
            {
                ShootAtPlayer();
                _shootInterval = 2.0f;
            }
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
        float angle = (Mathf.Atan2(playerRef.transform.position.y - transform.position.y,
            playerRef.transform.position.x - transform.position.x) - Mathf.PI / 2 ) * Mathf.Rad2Deg;

        Debug.Log(playerRef.transform.position);

        Instantiate(bullet, transform.GetChild(0).transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "bullet")
        {
            GameManagerScript.Instance.UpdatePlayerScore(100);
            GameManagerScript.Instance.RemoveUFO();

            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(otherObject.gameObject);
            Destroy(gameObject);
        }

        if (otherObject.gameObject.tag == "asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            GameManagerScript.Instance.RemoveUFO();

            Destroy(gameObject);
        }
    }

}
