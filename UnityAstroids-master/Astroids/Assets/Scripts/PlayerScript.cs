
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public float playerSpeed;
	public float turnSpeed;
	public static int playerLives;

	public static int playerScore = 0;

	public Rigidbody bullet;

	public Transform explosion;

    private Transform child;

	// Use this for initialization
	void Start () 
	{
		playerLives = 3;
		playerScore = 0;
        child = transform.GetChild(0);
	    child.renderer.enabled = false;


	}
	
	// Update is called once per frame
	void Update ()
	{
	    HandlePlayerInput();

	    CheckForWrap();

        //Win/Lose conditions
		if(playerScore >= 2500)
		{
			Application.LoadLevel(3);
		}

		if(playerLives <= 0)
		{
			Application.LoadLevel(2);
		}

	}

    private void HandlePlayerInput()
    {
        //move player
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.AddForce(transform.localRotation * Vector3.up * playerSpeed);
            child.renderer.enabled = true;
        }
        else
        {
            child.renderer.enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddTorque(Vector3.back * turnSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddTorque(Vector3.forward * turnSpeed);
        }

        //Fire bullets
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody tempBullet;
            tempBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
        }

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

    //Collision between player and enemies
	void OnTriggerEnter(Collider otherObject)
	{
		if (otherObject.gameObject.tag == "enemy")
		{
			Transform tempExplosion;

			tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(this.gameObject);

			playerLives--;
		}

	    if (otherObject.gameObject.tag == "asteroid")
	    {
	        Transform tempExplosion;

	        tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;
            
            Destroy(this.gameObject);

	        playerLives--;
	    }
	}
}
