using System;
//using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
    public Transform explosion;
	public float playerSpeed;
	public float turnSpeed;

	private int playerLives;
    private bool isAlive;
	private int playerScore = 0;
    public Transform thruster;
    private Renderer thrusterRendercmp;
    private Rigidbody2D rgdBody2D;

	// Use this for initialization
	void Start () 
	{
        playerLives = 3;
        playerScore = 0;
        thrusterRendercmp = thruster.GetComponent<Renderer>();
        thrusterRendercmp.enabled = false;
        rgdBody2D = GetComponent<Rigidbody2D>();
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
            rgdBody2D.AddRelativeForce(Vector2.up * playerSpeed, ForceMode2D.Force);
            thrusterRendercmp.enabled = true;
        }
        else
        {
            thrusterRendercmp.enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rgdBody2D.AddTorque(1.0f * -turnSpeed, ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rgdBody2D.AddTorque(1.0f * turnSpeed, ForceMode2D.Force);
        }

    }

    private void UpdatePlayerStatus()
    {
        GameManagerScript.Instance.CheckPlayerStatus(isAlive);
    }

    public bool isPlayerAlive()
    {
        return isAlive;
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