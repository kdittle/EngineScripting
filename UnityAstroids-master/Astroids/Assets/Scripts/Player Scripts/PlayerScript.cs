using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
    public GameObject gameManager;
    public GameManagerScript gmScript;

	public float playerSpeed;
	public float turnSpeed;
	public int playerLives;
    public bool isAlive;

	public int playerScore = 0;

	public Transform explosion;

    private Transform thruster;

    //Try and find the game manager
    void Awake()
    {
        try
        {
            gmScript = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }


        playerLives = 3;
        playerScore = 0;
        thruster = transform.FindChild("ShipThruster");
        thruster.renderer.enabled = false;
        isAlive = true;
    }

	// Use this for initialization
	void Start () 
	{
        //find or create the game manager!
        if (gmScript == null)
        {
            Debug.Log("There is no Game Manager");
            Debug.Log("Creating Game Manager");
            Instantiate(gameManager);

        }
        else
        {
            Debug.Log("Game Manager found.");
        }

	    
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        //Handle input
	    HandlePlayerInput();

	}

    private void HandlePlayerInput()
    {
        //move player
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rigidbody2D.AddRelativeForce(Vector2.up * playerSpeed, ForceMode2D.Force);
            thruster.renderer.enabled = true;
        }
        else
        {
            thruster.renderer.enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rigidbody2D.AddTorque(1.0f * -turnSpeed, ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rigidbody2D.AddTorque(1.0f * turnSpeed, ForceMode2D.Force);
        }

    }

    private void UpdatePlayerStatus()
    {
        GameObject.FindGameObjectWithTag("Game Manager").SendMessage("CheckPlayerStatus", isAlive);
    }

    //Collision between player and enemies
    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "enemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(this.gameObject);

            //playerLives--;
            isAlive = false;
            UpdatePlayerStatus();
        }

        if (otherObject.gameObject.tag == "asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(this.gameObject);

            //playerLives--;
            isAlive = false;
            UpdatePlayerStatus();
        }
    }

}