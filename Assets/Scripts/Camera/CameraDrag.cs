using UnityEngine;

public class CameraDrag : MonoBehaviour
{
  [Header("Drag Variables")]
  public float dragSpeed = 2;
  private Vector2 dragMouseOrigin;
  private Vector3 dragCameraOrigin;
  private Vector2 mousePosition;

  [Header("Mouse Boundaries")]
  private float rightOfScreen;
  private float leftOfScreen;
  private float topOfScreen;
  private float bottomOfScreen;

  [Header("Lets know if were dragging")]
  public bool draggingCamera;

  private void Start()
  {
    rightOfScreen = Screen.width;
    leftOfScreen = 0;
    topOfScreen = Screen.height;
    bottomOfScreen = 0;
  }

  void Update ()
  {
    //OthersWay2();
    FindCameraDrag();
  }

  // We are doing all of our movement based off the origin
  // the idea is they move in relation to that spot so if they move right 3 units then they move back left 3 units they will be back at that point

  private void FindCameraDrag()
  {
    mousePosition = Input.mousePosition;

    // get the origin of the mouse and camera and lock the camera to the screen then return so were not moving on our first frame
    if (Input.GetMouseButtonDown(2))
    {
      dragMouseOrigin = Input.mousePosition;
      dragCameraOrigin = transform.position;
      Cursor.lockState = CursorLockMode.Confined;
      return;
    }

    // while were dragging set the bool to true
    if (Input.GetMouseButton(2))
    {
      draggingCamera = true;

      // find the movement on the axis'. Make sure its within bounds of the screen
      if (mousePosition.x > leftOfScreen && mousePosition.x < rightOfScreen &&
          mousePosition.y > bottomOfScreen && mousePosition.y < topOfScreen)
      {
        Vector3 changeInMousePosition = new Vector3();

        changeInMousePosition = Camera.main.ScreenToViewportPoint(mousePosition - dragMouseOrigin);

        // multiply each variable by the drag speed
        Vector3 amountMovedThisFrame = new Vector3(-changeInMousePosition.x * dragSpeed, 0, -changeInMousePosition.y * dragSpeed);

        // then add the camera origin by the ammount moved this frame
        transform.position = dragCameraOrigin + amountMovedThisFrame;

         
      }
    }
    else
    {
      draggingCamera = false;
    }
  }

  //private void OthersWay()
  //{
  //  if (Input.GetMouseButtonDown(0))
  //  {
  //    dragMouseOrigin = Input.mousePosition;
  //    return;
  //  }

  //  if (!Input.GetMouseButtonUp(0)) return;

  //  if (Input.GetMouseButton(0))
  //  {
  //    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragMouseOrigin);
  //    Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
  //    transform.Translate(move, Space.World);

  //  }
  //}

  //private void OthersWay2()
  //{
  //  Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

  //  float left = Screen.width * 0.2F;
  //  float right = Screen.width - (Screen.width * 0.2F);

  //  if (mousePosition.x < left)
  //  {
  //    draggingCamera = true;
  //  }

  //  else if (mousePosition.x > right)
  //  {
  //    draggingCamera = true;
  //  }

  //  if (draggingCamera)
  //  {
  //    if (Input.GetMouseButtonDown(2))
  //    {
  //      dragMouseOrigin = Input.mousePosition;
  //      return;
  //    }

  //    if (!Input.GetMouseButton(2)) return;

  //    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragMouseOrigin);
  //    Vector3 move = new Vector3(pos.x * dragSpeed, 0, 0);

  //    transform.Translate(move, Space.World);
  //  }

  //}
}
