using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameManagerScript : MonoBehaviour 
{
    public int testInt = 5;
    

	// Use this for initialization
	void Start () 
    {
        GameObject.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
