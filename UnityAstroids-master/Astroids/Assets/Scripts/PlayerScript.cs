
using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public float playerSpeed;
	public float turnSpeed;
	public static int playerLives;
    public static bool isAlive;

	public static int playerScore = 0;

	public Rigidbody2D bullet;

	public Transform explosion;

    private Vector3 newPosition;

    private Transform fireLocation;
    private Transform thruster;

    private Renderer[] renderers;
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private Vector3 ScreenBottomLeft;
    private Vector3 ScreenTopRight;
    private float screenWidth;
    private float screenHeight;
    private Camera cam;

    private Transform[] ghosts = new Transform[8];

	// Use this for initialization
	void Start () 
	{
		playerLives = 3;
		playerScore = 0;
	    thruster = transform.FindChild("ShipThruster");
	    thruster.renderer.enabled = false;
	    fireLocation = transform.FindChild("FireLocation");
	    isAlive = true;

	    renderers = GetComponentsInChildren<Renderer>();

        //get main camera
	    cam = Camera.main;

        //screen bottom left
	    ScreenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));

        //screen top right
	    ScreenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        //get screen width and height
	    screenWidth = ScreenTopRight.x - ScreenBottomLeft.x;
	    screenHeight = ScreenTopRight.y - ScreenBottomLeft.y;

        //create ghost ships
        CreatGhostShips();
	}
	
	// Update is called once per frame
	void Update ()
	{
        //Handle input
	    HandlePlayerInput();

        //Swap ships for wrapping
        SwapShips();

        //Win/Lose conditions
		if(playerScore >= 2500)
		{
			Application.LoadLevel(3);
		}

		if(playerLives <= 0)
		{
			Application.LoadLevel(2);
		}

	}

    //Check if there are renderers in renderers
    bool CheckRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }

        return false;
    }

    private void HandlePlayerInput()
    {
        //move player
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody2D.AddRelativeForce(Vector2.up * playerSpeed, ForceMode2D.Force);
            thruster.renderer.enabled = true;
        }
        else
        {
            thruster.renderer.enabled = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody2D.AddTorque(1.0f * -turnSpeed, ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody2D.AddTorque(1.0f * turnSpeed, ForceMode2D.Force);
        }

        //Fire bullets
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D tempBullet;
            tempBullet = Instantiate(bullet, fireLocation.position, fireLocation.rotation) as Rigidbody2D;
        }

    }

    //Create ghost ships for wrapping
    void CreatGhostShips()
    {
        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

            if (ghosts[i] != null) 
                DestroyImmediate(ghosts[i], true);
        }
    }

    //positions the ghost ships for wrapping
    void PositionGhostShips()
    {
        Vector3 ghostPosition = transform.position;

        //position ghosts clockwaise behind the edges of the screen

        //far right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        //bottom right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        //bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }
    }

    //Swap ghost ships for wrapping
    void SwapShips()
    {
        foreach (Transform ghost in ghosts)
        {
            if (ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
                ghost.position.y < screenHeight && ghost.position.y > -screenHeight)
            {
                transform.position = ghost.position;
                break;
            }
        }

        PositionGhostShips();
    }

    //Collision between player and enemies
	void OnTriggerEnter(Collider otherObject)
	{
		if (otherObject.gameObject.tag == "enemy")
		{
			Transform tempExplosion;

			tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;

            Destroy(this.gameObject);

			playerLives--;
		    isAlive = false;
		}

	    if (otherObject.gameObject.tag == "asteroid")
	    {
	        Transform tempExplosion;

	        tempExplosion = Instantiate(explosion, transform.position, transform.rotation) as Transform;
            
            Destroy(this.gameObject);

	        playerLives--;
	        isAlive = false;
	    }
	}
}
