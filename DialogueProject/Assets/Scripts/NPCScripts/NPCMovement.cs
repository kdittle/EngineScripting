using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour 
{
	public enum NPCState
	{
		Idle,
		Talking,
		Walking,
		Transitioning
	};

	public NPCState _NPCState = new NPCState();

	public GameObject p_CurNode;
	public GameObject p_ClosestNode;
	public List<GameObject> PathList;

	// Use this for initialization
	void Start () 
	{
		//Set the npc state to idle right away
		_NPCState = NPCState.Idle;

		PathList = new List<GameObject> ();

		//Gather all the nodes for the path for the NPC to follow
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("NPCNode")) 
		{
			PathList.Add (g);
		}
		p_CurNode = PathList [0];
		//Just make the first node a game object.
		//p_CurNode = PathList [0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the NPC is walking do this
		if (_NPCState == NPCState.Walking)
		{
			//Find the closest node.
			foreach(GameObject node in PathList)
			{
				float closestDistance = Mathf.Infinity;

				if((Vector3.Distance(p_CurNode.transform.position, node.transform.position)) < closestDistance)
				{
					p_ClosestNode = node;
				}
			}

			transform.LookAt(p_ClosestNode.transform.position);
			transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10);

//			if(transform.position == p_ClosestNode.transform.position)
//			{
//				_NPCState = NPCState.Idle;
//				p_CurNode = p_ClosestNode;
//				PathList.Remove(p_ClosestNode);
//				p_ClosestNode = null;
//			}
		}
	}

}
