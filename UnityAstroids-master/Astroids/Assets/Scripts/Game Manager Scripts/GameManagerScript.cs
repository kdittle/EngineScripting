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

    private int _asteroidCount = 5;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        //Win/Lose conditions
        if (PlayerScript.playerScore >= PointsToWin)
        {
            Application.LoadLevel(3);
        }

        if (PlayerScript.playerLives <= 0)
        {
            Application.LoadLevel(2);
        }
	}

    public void RemoveAsteroidFromCount()
    {
        _asteroidCount--;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 50), "Score: " + PlayerScript.playerScore);

        GUI.Label(new Rect(10, 30, 200, 50), "Lives: " + PlayerScript.playerLives);
    }
}
