
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public float playerSpeed;
	public float turnSpeed;
	public static int playerLives;
    public static bool isAlive;

	public static int playerScore = 0;

	public Rigidbody bullet;

	public Transform explosion;

    private Transform child;

    private Vector3 newPosition;

	// Use this for initialization
	void Start () 
	{
		playerLives = 3;
		playerScore = 0;
	    child = transform.GetChild(1);
	    child.renderer.enabled = false;
	    isAlive = true;
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
            rigidbody2D.AddRelativeForce(Vector2.up * playerSpeed, ForceMode2D.Force);
            child.renderer.enabled = true;
        }
        else
        {
            child.renderer.enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody2D.AddTorque(1.0f * -turnSpeed, ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody2D.AddTorque(1.0f * turnSpeed, ForceMode2D.Force);
        }

        //Fire bullets
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Rigidbody tempBullet;
            //tempBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
        }

    }

    private void CheckForWrap()
    {
        //Check if player has gone too high, move him to the bottom
        if (transform.position.y > 12.5)
        {
            newPosition = new Vector3(0, -12.5f, 0);

            transform.position = newPosition;
        }

        //Check if player has gone too low, move him to top
        if (transform.position.y < -12.5)
        {
            newPosition = new Vector3(0, 12.5f, 0);

            transform.position = newPosition;
        }

        //Check if player has gone too far right, move him to left
        if (transform.position.x > 25)
        {
            newPosition = new Vector3(-25.0f, 0, 0);

            transform.position = newPosition;
        }

        //Check if player has gone too far left, move him to the right
        if (transform.position.x < -25.0)
        {
            newPosition = new Vector3(25.0f, 0, 0);

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
		    isAlive = false;
		}

	    if (otherObject.gameObject.tag == "asteroid")
	    {
	        Transform tempExplosion;

	        tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;
            
            Destroy(this.gameObject);

	        playerLives--;
	        isAlive = false;
	    }
	}
}
