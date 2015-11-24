using UnityEngine;
using System.Collections;
using System;
//using UnityEditor;

public class BulletController : MonoBehaviour
{

    public float speed;

    public float destroyOffset = 40.0f;

    private Vector2 initialPosition;

    void Awake()
    {

    }

	// Use this for initialization
	void Start ()
	{
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(Vector2.Distance(initialPosition, transform.position) > destroyOffset)
        {
            Destroy(gameObject);
        }

    }
}
