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

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        //Win/Lose conditions
        if (PlayerObject.GetComponent<PlayerScript>().playerScore >= PointsToWin)
        {
            Application.LoadLevel(3);
        }

        if ((PlayerObject.GetComponent<PlayerScript>().playerLives <= 0))
        {
            Application.LoadLevel(2);
        }
	}

    public void CheckPlayerStatus(bool isPlayerAlive)
    {
        if(!isPlayerAlive)
        {
            PlayerObject.GetComponent<PlayerScript>().playerLives--;
        }
    }

    public void UpdatePlayerScore(int scoreToAdd)
    {
        PlayerObject.GetComponent<PlayerScript>().playerScore += 100;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 50), "Score: " + PlayerObject.GetComponent<PlayerScript>().playerScore);

        GUI.Label(new Rect(10, 30, 200, 50), "Lives: " + PlayerObject.GetComponent<PlayerScript>().playerLives);
    }
}
