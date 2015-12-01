using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
    public enum PState
    {
        Idle,
        Walking,
        Running,
        Talking
    };

    public float PlayerSpeed;
    public float PlayerTurnSpeed;
    public float PlayerJump;

    private bool _isInAir = false;
    private bool _firstJump = false;

    public PState m_PlayerState = new PState();

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (m_PlayerState != PState.Talking)
        {
            if (Input.GetKey(KeyCode.A))
            {
                //Rotate Character Left
                transform.Rotate(Vector3.down * PlayerTurnSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                //Rotate Character Right
                transform.Rotate(Vector3.up * PlayerTurnSpeed);
            }

            if (Input.GetKey(KeyCode.W))
            {
                //Move Character Forward
                transform.Translate(Vector3.forward * PlayerSpeed * Time.deltaTime);
                m_PlayerState = PState.Walking;
            }

            if (Input.GetKey(KeyCode.S))
            {
                //Move Character Backward
                transform.Translate(Vector3.back * PlayerSpeed * Time.deltaTime);
                m_PlayerState = PState.Walking;
            }

            //Double jump feature. Has to be checked first so it runs on the second update after an inital jump
            if (Input.GetKeyDown(KeyCode.Space) && _isInAir && _firstJump)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * PlayerJump);
                _firstJump = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !_isInAir)
            {
                //Make Character Jump and also mark character as in air. This allows for only one jump at a time.
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * PlayerJump);
                _isInAir = true;
                _firstJump = true;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //If the player has hit the ground, set in air to false
        //allows player to jump again
        if (other.gameObject.tag == "Ground")
        {
            _isInAir = false;
            _firstJump = false;
        }
    }
}
