using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private GameObject player;
    private float posXShift;
    public float zoomSpeed = 10;
    private Vector3 originalCameraPos;
    private Vector3 originalPlayerPos;
    private Vector3 zoomScale;
    
    private Vector3 targetPosition;       // Target position for the camera
    public bool isZoomingIn = false;     // Flag to track zoom-in state
    public bool isZoomingOut = false;    // Flag to track zoom-out state

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        originalPlayerPos = player.transform.position;
        originalCameraPos = transform.position;
        zoomScale = (originalCameraPos - originalPlayerPos).normalized* 3;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{

        //    isZoomingIn = true;
        //    isZoomingOut = false;
        //    targetPosition = transform.position + zoomScale;
        //}

        //// Start zooming out when Z is released
        //if (Input.GetKeyUp(KeyCode.Z))
        //{

        //    targetPosition = originalCameraPos + (player.transform.position - originalPlayerPos);
        //    isZoomingOut = true;
        //    isZoomingIn = false;
        //}

        // Smoothly move the camera towards the target position
        if (isZoomingIn || isZoomingOut)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, zoomSpeed * Time.deltaTime);

            // Stop zooming when the camera is close to the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isZoomingIn = false;
                isZoomingOut = false;
            }
        }
    }
}
