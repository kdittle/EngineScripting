using UnityEngine;
using System.Collections;

public class PlayerSpawnScript : MonoBehaviour 
{

    private bool _spawnSafe = true;

	// Use this for initialization
	void Start () 
    {
        _spawnSafe = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool CheckSpawnStatus()
    {
        return _spawnSafe;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "asteroid" || other.gameObject.tag == "enemy")
        {
            _spawnSafe = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "asteroid" || other.gameObject.tag == "enemy")
        {
            _spawnSafe = true;
        }
    }
}
