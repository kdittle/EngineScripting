using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
	public float bulletSpeed;

	public Transform explosion;
    public Rigidbody2D bullet;

	// Use this for initialization
	void Start () 
	{

    }
	
	// Update is called once per frame
	void FixedUpdate () 
	{
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Instantiate(bullet, transform.position, transform.rotation);
	    }
	}
}
