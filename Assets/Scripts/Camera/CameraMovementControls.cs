using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementControls : MonoBehaviour
{
  [Header("Drag Settings")]
  [Range(0F,100F)]
  public float dragSpeed = 100;

  private Vector3 dragCameraOrigin = new Vector3();
  private Vector2 dragMouseOrigin = new Vector2();

  [Header("Edge Scroll Settings")]
  [Range(10,100)]
  public float edgeScrollRange = 20;
  [Range(1,20)]
  public float edgeScrollSpeed = 5;

  [Header("Key Scroll Settings")]
  [Range(10, 100)]
  public float keyScrollRange = 20;


  [Header("Mouse Boundaries")]
  private float rightOfScreen;
  private float leftOfScreen;
  private float topOfScreen;
  private float bottomOfScreen;

  [Header("Other variables")]
  private Vector2 mousePosition;
  private bool movingCamera;  // checks if we have moved our camera this frame already. We dont want two types of movement at a time
  private Camera thisCamera;

  private void Start()
  {
    thisCamera = GetComponent<Camera>();

    SetScreenBoundaries(); 
  }

  private void Update()
  {
    // set the mouse position
    mousePosition = Input.mousePosition;
    // set moving camera to false
    movingCamera = false;

    // lock the mouse when the player left clicks or middle clicks
    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(3))
    {
      Cursor.lockState = CursorLockMode.Confined;
    }

    // before we move the camera at all we should make sure the mouse is within screen view
    if (mousePosition.x >= leftOfScreen && mousePosition.x <= rightOfScreen &&
        mousePosition.y >= bottomOfScreen && mousePosition.y <= topOfScreen)
    {
      if (!movingCamera)
      {
        // Camera drag first
        CameraDrag();
      }

      if (!movingCamera)
      {
        CameraEdgeScroll();
      }

      if (!movingCamera)
      {
        CameraKeyScroll();
      }
    }


  }


  // sets the boundaries of the screen for the mouse
  public void SetScreenBoundaries()
  {
    rightOfScreen = Screen.width;
    topOfScreen = Screen.height;
    leftOfScreen = 0;
    bottomOfScreen = 0;
  }


  // does the camera drag
  private void CameraDrag()
  {
    // on the first time the player hits the middle mouse button we want to save their mouse position and camera position
    if (Input.GetMouseButtonDown(2))
    {
      dragMouseOrigin = mousePosition;
      dragCameraOrigin = transform.position;
      return;  // we return here cause we cant do anymore code on here and we dont want to
    }

    // find the mouse movement this frame
    if (Input.GetMouseButton(2))
    {
      movingCamera = true;

      // find the diffrence in camera mouse position
      Vector3 chagneInMousePosition = thisCamera.ScreenToViewportPoint(mousePosition - dragMouseOrigin);

      // multiply each axis by the drag speed then add it to the drag origin
      transform.position = dragCameraOrigin + new Vector3(-chagneInMousePosition.x * dragSpeed, 0, -chagneInMousePosition.y * dragSpeed);
    }
  }

  // does the edge scroll
  private void CameraEdgeScroll()
  {
    // just get an empyt thing
    Vector3 moveDirection = Vector3.zero;

    // find what direction were moving the camera
    if (mousePosition.x < leftOfScreen + edgeScrollRange) moveDirection += Vector3.left;
    if (mousePosition.x > rightOfScreen - edgeScrollRange) moveDirection += Vector3.right;
    if (mousePosition.y < bottomOfScreen + edgeScrollRange) moveDirection += Vector3.back;
    if (mousePosition.y > topOfScreen - edgeScrollRange) moveDirection += Vector3.forward;

    // update if were moveing
    if (moveDirection != Vector3.zero)
    {
      movingCamera = true;
    }

    // update our position based on what way we are moving
    transform.position += moveDirection.normalized * edgeScrollSpeed * Time.deltaTime;
  }

  // does key movement
  private void CameraKeyScroll()
  {
    // set a empty move direction
    Vector3 moveDirection = Vector3.zero;

    // check what direction the player is moving
    if (Input.GetKey(KeyCode.UpArrow)) moveDirection += Vector3.forward;
    if (Input.GetKey(KeyCode.DownArrow)) moveDirection += Vector3.back;
    if (Input.GetKey(KeyCode.LeftArrow)) moveDirection += Vector3.left;
    if (Input.GetKey(KeyCode.RightArrow)) moveDirection += Vector3.right;

    // move them that direction
    transform.position += moveDirection.normalized * keyScrollRange *Time.deltaTime;
  }
}
