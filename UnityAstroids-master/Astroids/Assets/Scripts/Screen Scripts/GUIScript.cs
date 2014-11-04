using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnGUI()
    {
            //Dispaly for score, lives, and enemies
		GUI.Label (new Rect (10, 10, 200, 50), "Score: " + PlayerScript.playerScore);

		GUI.Label (new Rect (10, 30, 200, 50), "Lives: " + PlayerScript.playerLives);
    }
}
