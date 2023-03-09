using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class CameraMovement : MonoBehaviour
// {
//     //who am I following?
//     public GameObject Player;
//     private Vector3 offset;
//     // Start is called before the first frame update
//     void Start()
//     {
//         offset=transform.position-Player.transform.position;
//     }

//     // Update is called once per frame
//     void LateUpdate()
//     {
//         transform.position = Player.transform.position + offset;
//     }
// }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //who am I following?
    public GameObject Player;

    // boundary objects
    public GameObject leftBoundary;
    public GameObject rightBoundary;
    public GameObject topBoundary;
    public GameObject bottomBoundary;

    private Vector3 offset;

    // Camera dimensions
    private float cameraWidth;
    private float cameraHeight;
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    // Boundary dimensions
    private float leftBoundaryX;
    private float rightBoundaryX;
    private float topBoundaryY;
    private float bottomBoundaryY;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate camera dimensions
        Camera camera = GetComponent<Camera>();
        cameraHeight = 2f * camera.orthographicSize;
        cameraWidth = cameraHeight * camera.aspect;
        cameraHalfHeight = camera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * camera.aspect;

        // Calculate boundary dimensions
        leftBoundaryX = leftBoundary.transform.position.x + leftBoundary.transform.localScale.x / 2f + cameraHalfWidth;
        rightBoundaryX = rightBoundary.transform.position.x - rightBoundary.transform.localScale.x / 2f - cameraHalfWidth;
        topBoundaryY = topBoundary.transform.position.y - topBoundary.transform.localScale.y / 2f - cameraHalfHeight;
        bottomBoundaryY = bottomBoundary.transform.position.y + bottomBoundary.transform.localScale.y / 2f + cameraHalfHeight;

        offset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get current camera position
        Vector3 currentPos = transform.position;

        // Get target camera position
        Vector3 targetPos = Player.transform.position + offset;

        // Limit camera movement within boundaries
        if (targetPos.x < leftBoundaryX) {
            targetPos.x = leftBoundaryX;
        } else if (targetPos.x > rightBoundaryX) {
            targetPos.x = rightBoundaryX;
        }
        if (targetPos.y < bottomBoundaryY) {
            targetPos.y = bottomBoundaryY;
        } else if (targetPos.y > topBoundaryY) {
            targetPos.y = topBoundaryY;
        }

        // Update camera position
        transform.position = targetPos;
    }
}




