
using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public float playerSpeed;
	public float turnSpeed;
	public static int playerLives;
    public static bool isAlive;

	public static int playerScore = 0;

	public Transform explosion;

    private Transform thruster;

	// Use this for initialization
	void Start () 
	{
		playerLives = 3;
		playerScore = 0;
	    thruster = transform.FindChild("ShipThruster");
	    thruster.renderer.enabled = false;
	    isAlive = true;

	    
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        //Handle input
	    HandlePlayerInput();

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
            thruster.renderer.enabled = true;
        }
        else
        {
            thruster.renderer.enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody2D.AddTorque(1.0f * -turnSpeed, ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody2D.AddTorque(1.0f * turnSpeed, ForceMode2D.Force);
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
