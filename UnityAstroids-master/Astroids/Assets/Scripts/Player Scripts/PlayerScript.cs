using System;
//using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
    public GameObject gameManager;
    public GameManagerScript gmScript;
    public Transform explosion;
	public float playerSpeed;
	public float turnSpeed;

	private int playerLives;
    private bool isAlive;
	private int playerScore = 0;
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
        thruster = transform.Find("ShipThruster");
        thruster.GetComponent<Renderer>().enabled = false;
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

        playerLives = 3;
        playerScore = 0;
        thruster = transform.Find("ShipThruster");
        thruster.GetComponent<Renderer>().enabled = false;
        isAlive = true;
	    
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
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * playerSpeed, ForceMode2D.Force);
            thruster.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            thruster.GetComponent<Renderer>().enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddTorque(1.0f * -turnSpeed, ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().AddTorque(1.0f * turnSpeed, ForceMode2D.Force);
        }

    }

    private void UpdatePlayerStatus()
    {
        GameObject.FindGameObjectWithTag("Game Manager").SendMessage("CheckPlayerStatus", isAlive);
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }

    public void RemovePlayerLife()
    {
        playerLives--;
    }

    public void AddToScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
    }

    public void ResetPlayer()
    {
        playerScore = 0;
        playerLives = 3;
    }

    public void ResetPlayerPosition()
    {
        transform.position.Set(0.0f, 0.0f, 0.0f);
    }

    public void ResetPlayerLives()
    {
        playerLives = 3;
    }

    public void ResetPlayerScore()
    {
        playerScore = 0;
    }

    //Collision between player and enemies
    void OnCollisionEnter2D(Collision2D otherObject)
    {
        if (otherObject.gameObject.tag == "enemy")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);
            Destroy(otherObject.gameObject);

            isAlive = false;
            UpdatePlayerStatus();
        }

        if (otherObject.gameObject.tag == "asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);

            isAlive = false;
            UpdatePlayerStatus();
        }

        if (otherObject.gameObject.tag == "alienBullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);

            isAlive = false;
            UpdatePlayerStatus();
        }
    }

}