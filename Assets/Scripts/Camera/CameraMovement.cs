using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public Vector3 previousMousePosition = new Vector3();
  public float mouseXDragSensitivity;
  public float mouseYDragSensitivity;

  public float _mouseChangeX;
  public float MouseChangeX
  {
    get { return _mouseChangeX; }
    set
    {
      if (value < -2)
      {
        _mouseChangeX = -2;
        return;
      }
      if (value > 2)
      {
        _mouseChangeX = 2;
        return;
      }

      _mouseChangeX = value;
    }
  }

  public float _mouseChangeY;
  public float MouseChangeY
  {
    get { return _mouseChangeY; }
    set
    {
      if (value < -2)
      {
        _mouseChangeY = -2;
        return;
      }
      if (value > 2)
      {
        _mouseChangeY = 2;
        return;
      }

      _mouseChangeY = value;
    }
  }

  void Update ()
  {
    MouseChangeX = 0F;
    MouseChangeY = 0F;

    Vector3 newMousePosition = Input.mousePosition;

    // check if your pressing middle mouse button
		if (Input.GetMouseButton(2))
    {
      // get the changes
      MouseChangeX = (newMousePosition.x - previousMousePosition.x) * Time.deltaTime * mouseXDragSensitivity;
      MouseChangeY = (newMousePosition.y - previousMousePosition.y) * Time.deltaTime * mouseYDragSensitivity;

      Vector3 changePerFrame = new Vector3();

      // add mouseX chagne to change per frame
      changePerFrame += new Vector3(-MouseChangeX, 0, 0);

      // add mouseY change to change per frame
      changePerFrame += new Vector3(0, 0, -MouseChangeY);

      transform.position += changePerFrame;

    }

    previousMousePosition = newMousePosition;
	}
}
