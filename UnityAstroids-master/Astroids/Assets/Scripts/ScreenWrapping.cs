using UnityEngine;
using System.Collections;

public class ScreenWrapping : MonoBehaviour
{
    private Renderer[] renderers;
    //private bool isWrappingX = false;
    //private bool isWrappingY = false;
    private Vector3 ScreenBottomLeft;
    private Vector3 ScreenTopRight;
    private float screenWidth;
    private float screenHeight;
    private Camera cam;

    private Transform[] ghosts = new Transform[8];

	// Use this for initialization
	void Start () 
    {
        renderers = GetComponentsInChildren<Renderer>();

        //create ghost ships
        CreatGhostShips();

        //get main camera
        cam = Camera.main;

        //screen bottom left
        ScreenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));

        //screen top right
        ScreenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        //get screen width and height
        screenWidth = ScreenTopRight.x - ScreenBottomLeft.x;
        screenHeight = ScreenTopRight.y - ScreenBottomLeft.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        //Swap ships for wrapping
        SwapShips();
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

    //Create ghost ships for wrapping
    void CreatGhostShips()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            if (this.transform != null)
            {
                ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

                if (ghosts[i].transform != null)
                {
                    DestroyImmediate(ghosts[i].GetComponent<ScreenWrapping>(), true);
                    //DestroyImmediate(ghosts[i].GetComponentInChildren<BulletScript>());
                }
            }
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
}
