using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidScript : MonoBehaviour
{
    public int maxNumAsteroids;
    List<Rigidbody> asteroids = new List<Rigidbody>();


	// Use this for initialization
	void Start ()
	{
	    maxNumAsteroids = 10;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    SpawnAsteroids();
	}

    void SpawnAsteroids()
    {
        if (asteroids.Count < maxNumAsteroids)
        {
            for (int i = 0; i < 100; i++)
            {
                
            }

            float temp = Random.Range(-6, 6);
            Rigidbody newAsteroid = Instantiate(this.transform, new Vector3(temp, 6, 0), this.transform.rotation) as Rigidbody;
            asteroids.Add(newAsteroid);
            MoveAsteroids();
        }
    }

    void MoveAsteroids()
    {
        //move enemy
        float amtToMove = 10 * Time.deltaTime;

        foreach (Rigidbody asteroid in asteroids)
        {
            asteroid.transform.Translate(Vector3.down * Time.deltaTime);
        }
    }
}
