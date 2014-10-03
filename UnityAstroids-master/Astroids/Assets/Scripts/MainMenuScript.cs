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
		const string instructionText = "Instructions\nUp Arrow to thrust forward \nLeft and Right arrows to rotate \nSpacebar to fire";

		GUI.Label(new Rect(10, 10, 200, 200), instructionText);

		if (GUI.Button (new Rect (10, 75, 200, 50), "Start Game"))
		{
			Application.LoadLevel(1);
		}
	}
}
