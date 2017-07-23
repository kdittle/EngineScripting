using UnityEngine;
//using UnityEditor;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{

    public float PointsToWin = 2500;
    public GameObject PlayerObject;
    public GameObject PlayerSpawnObject;
    public GameObject AsteroidSpawnObject;
    public GameObject[] AsteroidList;
    public GameObject ufoObjcet;
    public GUIStyle Style;

    private bool _spawnSafe;
    private bool _isPlayerDead;
    private PlayerScript playerScript;

    private int _intialAsteroidCount = 4;
    private int _curAsteroidCount = 0;
    private float _UFOspawnTimer = 5.0f;
    private int _curUFOCount = 0;
    private int _maxUFOCount = 2;

    private int _level = 1;
    private int _highScore = 0;
    private bool _winDisplay = false;
    private bool _loseDisplay = false;
    private bool _pauseGame = false;

    private static GameManagerScript s_instance = null;

    public static GameManagerScript Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType(typeof(GameManagerScript)) as GameManagerScript;

                return s_instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        //don;t destroy the game manager. IT needs to stick around
        DontDestroyOnLoad(gameObject);
        playerScript = PlayerObject.GetComponent<PlayerScript>();
        playerScript.ResetPlayer(); //reset the player each time a new game is started
        _isPlayerDead = false;
        _winDisplay = false;
        _loseDisplay = false;
        _pauseGame = false;

        _intialAsteroidCount = 4;
        _curAsteroidCount = 0;
        _highScore = 0;
        _UFOspawnTimer = 5.0f;

        //spawn asteroids
        SetUpAsteroids();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_pauseGame)
        {
            _UFOspawnTimer -= Time.deltaTime;

            if (_UFOspawnTimer <= 0 && GameObject.FindGameObjectWithTag("enemy") == null
                && _curUFOCount < _maxUFOCount)
            {
                SpawnUFO();
                _UFOspawnTimer = 5.0f;
            }

            //respawn player if he is dead
            if (_isPlayerDead && PlayerSpawnObject.GetComponent<PlayerSpawnScript>().CheckSpawnStatus())
            {
                RespawnPlayer();
            }

            //Win/Lose conditions
            if (_curAsteroidCount <= 0)
            {
                _winDisplay = true;
                _pauseGame = true;
            }

            if ((playerScript.GetPlayerLives() < 0))
            {
                _loseDisplay = true;
                _pauseGame = true;
            }
        }
    }

    void SetUpAsteroids()
    {
        //sets up the asteroids
        for (int i = 0; i < _intialAsteroidCount; i++)
        {
            SpawnAsteroids();
            _curAsteroidCount++;
        }

    }

    void SpawnAsteroids()
    {
        //randomly spawn the asteroids
        float x = Random.Range(-20, 20);
        float y = Random.Range(-15, 15);
        Vector2 tempPos = new Vector2(x, y);

        //check the distance from the player, keepds them from spawning too close to thep player
        if (Vector2.Distance(PlayerObject.transform.position, tempPos) > 5)
            Instantiate(AsteroidList[Random.Range(0, AsteroidList.Length)], tempPos, new Quaternion());
        else
            SpawnAsteroids(); //if they are too close to the player, just call the method again and again.
    }

    private void SpawnUFO()
    {
        float x = Random.Range(-20, 20);
        float y = Random.Range(-15, 15);
        Vector2 tempPos = new Vector2(x, y);

        if (Vector2.Distance(PlayerObject.transform.position, tempPos) > 5)
        {
            Instantiate(ufoObjcet, tempPos, new Quaternion());
            _curUFOCount++;
        }
        else
            SpawnUFO();
    }

    public void RemoveUFO()
    {
        _curUFOCount--;
    }

    public void UpdateAsteroidCount(int countMod)
    {
        //updates the number of asteroids on the screen
        _curAsteroidCount += countMod;
    }

    //check if the palyer is alive or dead
    public void CheckPlayerStatus(bool isPlayerAlive)
    {
        if (!isPlayerAlive)
        {
            _isPlayerDead = true;
            //if player dies, remove a life
            playerScript.RemovePlayerLife();
        }
    }

    //updates the player score
    public void UpdatePlayerScore(int scoreToAdd)
    {
        playerScript.AddToScore(scoreToAdd);
        _highScore = playerScript.GetPlayerScore();
    }

    //respawn the player
    public void RespawnPlayer()
    {
        Instantiate(PlayerObject);
        _isPlayerDead = false;
    }

    //handles starting the next level
    private void StartNextLevel()
    {
        _level++;
        _intialAsteroidCount++;
        _curAsteroidCount = 0;

        playerScript.ResetPlayerPosition();

        //clean up all the explosion prefabs that stick around
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("explosion"))
        {
            Destroy(obj);
        }

        _winDisplay = false;
        _loseDisplay = false;
        _pauseGame = false;
        SetUpAsteroids();
    }

    private void KeepPlaying()
    {
        _curAsteroidCount = 0;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("asteroid"))
        {
            Destroy(obj.gameObject);
        }

        playerScript.ResetPlayerPosition();
        playerScript.ResetPlayerLives();
        playerScript.ResetPlayerScore();
        _winDisplay = false;
        _loseDisplay = false;
        SetUpAsteroids();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("explosion"))
        {
            Destroy(obj);
        }

        _pauseGame = false;
    }

    void OnGUI()
    {

        //display stuffs
        GUI.Label(new Rect(10, 10, 200, 50), "Score: " + playerScript.GetPlayerScore(), Style);

        GUI.Label(new Rect(10, 30, 200, 50), "Lives: " + playerScript.GetPlayerLives(), Style);

        GUI.Label(new Rect(Screen.width / 2, 0, 200, 100), "High Score: " + _highScore, Style);

        if(_winDisplay)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("asteroid"))
            {
                Destroy(obj.gameObject);
            }

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("enemy"))
            {
                Destroy(obj.gameObject);
            }

            GUI.Label(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 50, 200, 50), "You Win!", Style);
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "Next Level", Style))
            {
                StartNextLevel();
            }

        }

        if(_loseDisplay)
        {
            GUI.Label(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 50, 200, 50), "You Lose!", Style);
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 50), "Restart", Style))
            {
                KeepPlaying();
            }
        }
    }
}
