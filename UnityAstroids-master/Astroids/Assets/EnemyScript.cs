using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour 
{
	public Transform target;
	public float enemySpeed;
	public float maxNumEnemies = 10;
	public static float numEnemies = 0;
	List<Rigidbody> enemies = new List<Rigidbody>();


	// Use this for initialization
	void Start () 
	{
		numEnemies = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//SpawnEnemy ();
		//MoveEnemy ();

		if (transform.position.y <= -7) 
		{
			Destroy(gameObject);
		}
	}

	void SpawnEnemy()
	{
		
		while (numEnemies < maxNumEnemies ) 
		{
			Rigidbody newEnemy;
			float temp = Random.Range(-6, 6);
			newEnemy = Instantiate(this.transform, new Vector3(temp, 6, 0), this.transform.rotation) as Rigidbody;
			enemies.Add(newEnemy);
			numEnemies++;
		}
	}

	void MoveEnemy()
	{
		//move enemy
		float amtToMove = enemySpeed * Time.deltaTime;

		foreach (Rigidbody enemy in enemies) 
		{
			enemy.transform.Translate(Vector3.down * Time.deltaTime);
		}

		//transform.Translate (Vector3.down * amtToMove);
	}
}
