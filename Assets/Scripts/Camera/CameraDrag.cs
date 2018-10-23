using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
  [Header("others way")]
  public float dragSpeed = 2;
  public Vector3 dragOrigin;
  public Vector2 mousePosition;

  [Space()]
  public float right;
  public float left;
  public float top;
  public float bottom;

  public bool cameraDragging = true;

  private void Start()
  {
    right = Screen.width;
    left = 0;
    top = Screen.height;
    bottom = 0;
  }

  void Update ()
  {
    mousePosition = Input.mousePosition;
    //OthersWay2();
    MoveMouse();
  }

  private void MoveMouse()
  {
    // when the player presses down the grip get that origin
    if (Input.GetMouseButtonDown(2))
    {
      dragOrigin = Input.mousePosition;
      return;
    }

    // if were currently dragging
    if (Input.GetMouseButton(2))
    {
      // do the movement of the x axis
      if (mousePosition.x > left && mousePosition.x < right)
      {
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, 0);

        transform.position += move;
      }
    }
    // We are doing all of our movement based off the origin
    // the idea is they move in relation to that spot so if they move right 3 units then they move back left 3 units they will be back at that point
  }

  private void OthersWay()
  {
    if (Input.GetMouseButtonDown(0))
    {
      dragOrigin = Input.mousePosition;
      return;
    }

    if (!Input.GetMouseButtonUp(0)) return;

    if (Input.GetMouseButton(0))
    {
      Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
      Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
      transform.Translate(move, Space.World);

    }
  }

  private void OthersWay2()
  {
    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

    float left = Screen.width * 0.2F;
    float right = Screen.width - (Screen.width * 0.2F);

    if (mousePosition.x < left)
    {
      cameraDragging = true;
    }

    else if (mousePosition.x > right)
    {
      cameraDragging = true;
    }

    if (cameraDragging)
    {
      if (Input.GetMouseButtonDown(2))
      {
        dragOrigin = Input.mousePosition;
        return;
      }

      if (!Input.GetMouseButton(2)) return;

      Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
      Vector3 move = new Vector3(pos.x * dragSpeed, 0, 0);

      transform.Translate(move, Space.World);
    }

  }
}
