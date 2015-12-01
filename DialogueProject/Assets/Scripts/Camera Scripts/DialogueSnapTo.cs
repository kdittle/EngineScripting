using UnityEngine;
using System.Collections;

public class DialogueSnapTo : MonoBehaviour
{
    private GameObject m_PlayerObject;



	// Use this for initialization
	void Start ()
    {
        m_PlayerObject = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If player is talking, snap camera to the dialgue position
	    if(m_PlayerObject.GetComponent<PlayerMovementScript>().m_PlayerState == PlayerMovementScript.PState.Talking)
        {
            transform.position = (GameObject.FindGameObjectWithTag("CameraSnap").transform.position);
            transform.LookAt(m_PlayerObject.transform.position);
        }
	}
}
