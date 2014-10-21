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
	void Update () 
	{
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Rigidbody2D newBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody2D;
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
            Destroy(otherObject.gameObject);
		}

        if (otherObject.gameObject.tag == "asteroid")
        {
            PlayerScript.playerScore += 100;

            Transform tempExplosion;

            tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(otherObject.gameObject);
            Destroy(gameObject);


            Transform tempTran;
            Transform tempTran2;

            tempTran = Instantiate(otherObject.gameObject.transform, otherObject.gameObject.transform.position, otherObject.gameObject.transform.rotation) as Transform;
            tempTran.transform.localScale = new Vector3(1.3f, 1.3f, 0.0f);

            tempTran2 = Instantiate(otherObject.gameObject.transform, new Vector3(otherObject.gameObject.transform.position.x + 1, otherObject.gameObject.transform.position.y + 1, 0.0f), 
                otherObject.gameObject.transform.rotation) as Transform;
            tempTran2.transform.localScale = new Vector3(1.1f, 1.1f, 0.0f);
        }
	}
}
