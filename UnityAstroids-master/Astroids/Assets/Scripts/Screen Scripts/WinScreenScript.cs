using UnityEngine;
using System.Collections;

public class WinScreenScript : MonoBehaviour 
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
		if (GUI.Button (new Rect (10, 10, 300, 50), "You won! Press to paly again")) 
		{
			Application.LoadLevel(1);
		}
	}
}
