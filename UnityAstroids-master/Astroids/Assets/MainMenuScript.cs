using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour 
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
		string instructionText = "Instructions\nLeft and Right arrows to move\nSpacebar to fire";

		GUI.Label(new Rect(10, 10, 200, 200), instructionText);

		if (GUI.Button (new Rect (10, 60, 200, 50), "Start Game"))
		{
			Application.LoadLevel(1);
		}
	}
}
