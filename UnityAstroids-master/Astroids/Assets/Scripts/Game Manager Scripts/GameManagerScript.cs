using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{

    public float PointsToWin = 2500;
    public GameObject PlayerObject;
    public GameObject PlayerSpawnObject;
    public GameObject AsteroidSpawnObject;
    public GameObject[] AsteroidList;

    private bool _spawnSafe;
    private bool _isPlayerDead;

    private int _intialAsteroidCount = 1;
    private int _curAsteroidCount = 0;

    private int _level = 1;
    private bool _winDisplay = false;
    private bool _loseDisplay = false;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        PlayerObject.GetComponent<PlayerScript>().ResetPlayer();
        _isPlayerDead = false;
        _winDisplay = false;
        _loseDisplay = false;

        _intialAsteroidCount = 5;
        _curAsteroidCount = 0;

        SetUpAsteroids();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (_isPlayerDead && PlayerSpawnObject.GetComponent<PlayerSpawnScript>().CheckSpawnStatus())
        {
            RespawnPlayer();
        }

        Debug.Log(_curAsteroidCount);

        //Win/Lose conditions
        if (_curAsteroidCount <= 0)
        {
            _winDisplay = true;
        }

        if ((PlayerObject.GetComponent<PlayerScript>().GetPlayerLives() <= 0))
        {
            _loseDisplay = false;
        }
    }

    void SetUpAsteroids()
    {
        for (int i = 0; i < _intialAsteroidCount; i++)
        {
            SpawnAsteroids();
            _curAsteroidCount++;
        }

    }

    void SpawnAsteroids()
    {
        float x = Random.Range(-20, 20);
        float y = Random.Range(-15, 15);
        Vector2 tempPos = new Vector2(x, y);

        if (Vector2.Distance(PlayerObject.transform.position, tempPos) > 5)
            Instantiate(AsteroidList[Random.Range(0, AsteroidList.Length)], tempPos, new Quaternion());
        else
            SpawnAsteroids();
    }

    public void CheckPlayerStatus(bool isPlayerAlive)
    {
        if (!isPlayerAlive)
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

    private void StartNextLevel()
    {
        _level++;
        _intialAsteroidCount++;
        _curAsteroidCount = 0;
        PlayerObject.GetComponent<PlayerScript>().ResetPlayerPosition();

        _winDisplay = false;
        _loseDisplay = false;
        SetUpAsteroids();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 50), "Score: " + PlayerObject.GetComponent<PlayerScript>().GetPlayerScore());

        GUI.Label(new Rect(10, 30, 200, 50), "Lives: " + PlayerObject.GetComponent<PlayerScript>().GetPlayerLives());

        if(_winDisplay)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("asteroid"))
            {
                Destroy(obj.gameObject);
            }

            GUI.Label(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 50, 200, 50), "You Win!");
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "Next Level"))
            {
                StartNextLevel();
            }

        }

        if(_loseDisplay)
        {
            GUI.Label(new Rect(10, 10, 200, 50), "You Lose!");
        }
    }
}
