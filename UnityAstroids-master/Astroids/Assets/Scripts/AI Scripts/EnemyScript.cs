using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour 
{
	public Transform target;
    public Rigidbody2D bullet;
    public Transform explosion;
    public float enemySpeed;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

    private void ShootAtPlayer()
    {
        Instantiate(bullet, transform.position, target.transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "bullet")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().playerScore += 100;

            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(otherObject.gameObject);
            Destroy(gameObject);
        }
    }

}
