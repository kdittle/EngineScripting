using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class FollowCameraScript : MonoBehaviour
{
    public float MinDistance;
    public float MaxDistance;
    public float xSpeed;
    public float ySpeed;
    public float distance;
    public float yMin;
    public float yMax;

    private float _x;
    private float _y;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y,
            player.transform.position.z - 10);

        //if(!PlayerUIScript.IsDisplayingMenu)
        //    distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, MinDistance, MaxDistance);

        if (Input.GetMouseButton(0) /*&& !PlayerUIScript.IsDisplayingMenu*/)
        {
            _x += Input.GetAxis("Mouse X") * xSpeed * distance * .02f;
            _y -= Input.GetAxis("Mouse Y") * ySpeed * .02f;

            _y = Mathf.Clamp(_y, yMin, yMax);
        }
        
        Quaternion rotation;
        Vector3 negDistance;
        Vector3 position;

        rotation = Quaternion.Euler(_y, _x, 0);

        negDistance = new Vector3(0.0f, 0.0f, -distance);
        position = rotation * negDistance + player.transform.position;

        transform.rotation = rotation;
        transform.position = position;

        if(Input.GetMouseButton(1) /*&& !PlayerUIScript.IsDisplayingMenu*/)
        {
            _x += Input.GetAxis("Mouse X") * xSpeed * distance * .02f;
            _y -= Input.GetAxis("Mouse Y") * ySpeed * .02f;

            _y = Mathf.Clamp(_y, yMin, yMax);


            rotation = Quaternion.Euler(_y, _x, 0);

            negDistance = new Vector3(0.0f, 0.0f, -distance);
            position = rotation * negDistance + player.transform.position;

            transform.rotation = rotation;
            transform.position = position;

            Quaternion playerRot = Quaternion.Euler(0.0f, _x, 0.0f);

            player.transform.rotation = playerRot;
        }
    }
}
