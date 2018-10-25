using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
  public LayerMask tileLayer;
  public float raycastRange = 20F;
  public Camera myCamera;

  public GameObject[] towers;

  private void Update()
  {
    if (Input.GetMouseButtonDown(1))
    {
      RaycastHit hit;
      Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit, raycastRange, tileLayer))
      {
        if (hit.transform.GetComponentInParent<TileInformation>())
        {
          TileInformation tileInfo = hit.transform.GetComponentInParent<TileInformation>();
          tileInfo.BuildTower(RandomTower());

        }
      }
    }
  }

  private GameObject RandomTower()
  {
    int randomTower = Random.Range(0, towers.Length);
    return towers[randomTower];
  }
}
