using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovementScript : MonoBehaviour
{
    public float dragSpeed = 0.1f;
    public float smoothTime = 0.3f; //time it takes to smooth the movement
    private Vector3 dragOrigin; //Initial touch position

    private Vector3 targetPosition; // desired position to go to
    private Vector3 velocity = Vector3.zero;

    private Vector3 moveDirection; //Direction to move the camera

    private bool isDragging = false; //Track dragging state

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandleTouchInput();

        //Smoothly move camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime); ;
    }

    void HandleTouchInput()
    {
        //if there is at least one touch
        if(Input.touchCount == 1)
        {
            Touch firstTouch = Input.GetTouch(0);

            //On touch start
            if (firstTouch.phase == TouchPhase.Began)
            {
                dragOrigin = firstTouch.position;
                isDragging = true;
            }

            //On touch drag
            if (firstTouch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 touchPosition = firstTouch.position;
                Vector3 direction = (dragOrigin - touchPosition).normalized * dragSpeed;

                moveDirection = new Vector3(-direction.y, 0, direction.x)*dragSpeed * Time.deltaTime;

                targetPosition += moveDirection;

                //update the dragOrigin to the current touch position
                dragOrigin = touchPosition;
            }

            if (firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled) 
            {
                isDragging = false;
            }
        }

        
    }
}

