using UnityEngine;
using System.Collections;

public class LoseScreenScript : MonoBehaviour 
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
		if(GUI.Button(new Rect(10, 10, 300, 50), "You Lost, Press to play again."))
		{
			Application.LoadLevel(1);
		}
	}
}
