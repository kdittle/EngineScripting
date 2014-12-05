using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{

    public float PointsToWin = 2500;
    public GameObject PlayerObject;
    public GameObject PlayerSpawnObject;
    public GameObject AsteroidSpawnObject;

    private bool _spawnSafe;
    private bool _isPlayerDead;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(gameObject);
        PlayerObject.GetComponent<PlayerScript>().ResetPlayer();
        _isPlayerDead = false;

	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        if(_isPlayerDead && PlayerSpawnObject.GetComponent<PlayerSpawnScript>().CheckSpawnStatus())
        {
            RespawnPlayer();
        }

        //Win/Lose conditions
        if (PlayerObject.GetComponent<PlayerScript>().GetPlayerScore() >= PointsToWin)
        {
            Application.LoadLevel(3);
        }

        if ((PlayerObject.GetComponent<PlayerScript>().GetPlayerLives() <= 0))
        {
            Application.LoadLevel(2);
        }
	}

    public void CheckPlayerStatus(bool isPlayerAlive)
    {
        if(!isPlayerAlive)
        {
            _isPlayerDead = true;
            PlayerObject.GetComponent<PlayerScript>().RemovePlayerLife();
        }
    }

    public void UpdatePlayerScore(int scoreToAdd)
    {
        PlayerObject.GetComponent<PlayerScript>().AddToScore(scoreToAdd);
    }

    public void RespawnPlayer()
    {
        Instantiate(PlayerObject);
        _isPlayerDead = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 50), "Score: " + PlayerObject.GetComponent<PlayerScript>().GetPlayerScore());

        GUI.Label(new Rect(10, 30, 200, 50), "Lives: " + PlayerObject.GetComponent<PlayerScript>().GetPlayerLives());
    }
}
