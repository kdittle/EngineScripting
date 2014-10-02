using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
	public float bulletSpeed;

	public Transform explosion;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move bullet
		float amtToMove = bulletSpeed * Time.deltaTime;

		transform.Translate (Vector3.up * amtToMove);

		if (transform.position.y >= 12 || transform.position.y <= -12 
            || transform.position.x >= 23 || transform.position.x <= -23)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider otherObject)
	{
		if(otherObject.gameObject.tag == "enemy")
		{
			PlayerScript.playerScore += 200;

			Transform tempExplosion;

			tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

			Destroy(gameObject);
		}

        if (otherObject.gameObject.tag == "asteroid")
        {
            PlayerScript.playerScore += 100;

            Transform tempExplosion;

            tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(gameObject);
        }
	}
}
