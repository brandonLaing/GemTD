using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEdgeScroll : MonoBehaviour
{
  public float cameraScrollSpeed = 5;
  public float scrollZoneRange = 20;

  private Vector3 mousePosition;

  [Header("Mouse Boundaries")]
  private float rightOfScreen;
  private float leftOfScreen;
  private float topOfScreen;
  private float bottomOfScreen;

  private void Start()
  {

    rightOfScreen = Screen.width;
    leftOfScreen = 0;
    topOfScreen = Screen.height;
    bottomOfScreen = 0;

  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Cursor.lockState = CursorLockMode.Confined;
    }

    mousePosition = Input.mousePosition;

    DoCameraEdgeScroll();
  }

  private void DoCameraEdgeScroll()
  {
    Vector3 moveDirection = new Vector3();

    // so if its within the screen and within the scroll area move it in that direction
    if (mousePosition.x > leftOfScreen && mousePosition.x < rightOfScreen &&
        mousePosition.y > bottomOfScreen && mousePosition.y < topOfScreen)
    {
      if (mousePosition.x < leftOfScreen + scrollZoneRange)
      {
        moveDirection += Vector3.left;
      }
      if (mousePosition.x > rightOfScreen - scrollZoneRange)
      {
        moveDirection += Vector3.right;
      }
      if (mousePosition.y < bottomOfScreen + scrollZoneRange)
      {
        moveDirection += Vector3.back;
      }
      if (mousePosition.y > topOfScreen - scrollZoneRange)
      {
        moveDirection += Vector3.forward;
      }

      transform.position += moveDirection * cameraScrollSpeed * Time.deltaTime;
    }
  }
}
