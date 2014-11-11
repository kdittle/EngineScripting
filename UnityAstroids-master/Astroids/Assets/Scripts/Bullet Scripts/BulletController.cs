using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class BulletController : MonoBehaviour
{

    public float speed;
    public float bulletLifeTime;

    private float curBulLife;

    void Awake()
    {

    }

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
    {
	    transform.Translate(Vector3.up * speed * Time.deltaTime);

	    curBulLife = Time.deltaTime;

	    if (curBulLife >= bulletLifeTime)
	    {
	        Destroy(transform.gameObject);
	        curBulLife = 0;
	    }
    }
}
