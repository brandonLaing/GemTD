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
    if (Input.anyKey)
    {
      Debug.LogWarning("keyHit");

    }

    mousePosition = Input.mousePosition;
  }

  private void DoCameraEdgeScroll()
  {
    if (mouse)
  }
}
