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

    private GameObject m_PlayerObject;

	// Use this for initialization
	void Start () 
    {
        m_PlayerObject = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
    private void Update()
    {
        if (m_PlayerObject.GetComponent<PlayerMovementScript>().m_PlayerState != PlayerMovementScript.PState.Talking)
        {
            transform.position = new Vector3(m_PlayerObject.transform.position.x, m_PlayerObject.transform.position.y,
                m_PlayerObject.transform.position.z - 10);

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
            position = rotation * negDistance + m_PlayerObject.transform.position;

            transform.rotation = rotation;
            transform.position = position;

            if (Input.GetMouseButton(1) /*&& !PlayerUIScript.IsDisplayingMenu*/)
            {
                _x += Input.GetAxis("Mouse X") * xSpeed * distance * .02f;
                _y -= Input.GetAxis("Mouse Y") * ySpeed * .02f;

                _y = Mathf.Clamp(_y, yMin, yMax);


                rotation = Quaternion.Euler(_y, _x, 0);

                negDistance = new Vector3(0.0f, 0.0f, -distance);
                position = rotation * negDistance + m_PlayerObject.transform.position;

                transform.rotation = rotation;
                transform.position = position;

                Quaternion playerRot = Quaternion.Euler(0.0f, _x, 0.0f);

                m_PlayerObject.transform.rotation = playerRot;
            }
        }
    }
}
