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
	    if (PlayerScript.isAlive)
	    {
	        transform.Translate(target.transform.position * enemySpeed * Time.deltaTime);

	        if ((transform.position.x - target.transform.position.x) <= 4.5f ||
	            (transform.position.y - target.transform.position.y) <= 4.5)
	        {
	            ShootAtPlayer();
	        }
	    }

	    CheckForWrap();
	}

    private void ShootAtPlayer()
    {
        Rigidbody tempBullet;
        tempBullet = Instantiate(bullet, transform.position, target.transform.rotation) as Rigidbody;
    }

    private void CheckForWrap()
    {
        //Check if player has gone too high, move him to the bottom
        if (transform.position.y > 12.5)
        {
            Vector3 newPosition = new Vector3(0, -12.5f, 0);

            transform.position = newPosition;
        }

        //Check if player has gone too low, move him to top
        if (transform.position.y < -12.5)
        {
            Vector3 newPosition = new Vector3(0, 12.5f, 0);

            transform.position = newPosition;
        }

        //Check if player has gone too far right, move him to left
        if (transform.position.x > 25)
        {
            Vector3 newPosition = new Vector3(-25.0f, 0, 0);

            transform.position = newPosition;
        }

        //Check if player has gone too far left, move him to the right
        if (transform.position.x < -25.0)
        {
            Vector3 newPosition = new Vector3(25.0f, 0, 0);

            transform.position = newPosition;
        }
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
        }
    }

}
