using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileInformation : MonoBehaviour
{
  [Space()]
  public TileNode myNode;
  [Space()]
  public GameObject tower;
  [Space()]
  public Vector3 offSet;

  public bool IsEmpty
  {
    get { return tower == null; }
  }

  public void BuildTower(GameObject tower)
  {
    if (IsEmpty)
    {
      Debug.Log(transform.name + " built a tower");
      this.tower = Instantiate(tower, transform.position + offSet, Quaternion.identity, this.transform);
      myNode.Block();
    }
  }

  public void DestroyTower()
  {
    if (!IsEmpty)
    {
      Debug.Log(transform.name + " destroyed a tower");
      Destroy(tower);
      tower = null;
      myNode.Unblock();
    }
  }
}
