using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerSpawn;
    public List<Asteroid> Asteroids;

    public int Score = 0;
    public int HighScore = 0;

    private bool bIsGamePaused;
    private bool bIsPlayerDead;


    private static GameManager s_Instance;

    public static GameManager Instance
    {
        get
        {
            if (s_Instance == null)
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            return s_Instance;
        }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
