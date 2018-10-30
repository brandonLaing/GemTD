using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
  public int numberOfRounds = 102;

  public LayerMask tileLayer;
  public float raycastRange = 20F;
  public Camera myCamera;

  [Header("Type Chances")]
  public float[] gemTypeChances;
  public float step = 1.28F;
  public float total = 0;

  [Header("Tier Chances")]
  public float[] gemTierChances;

  //public Dictionary<GameObject, float> towerChances = new Dictionary<GameObject, float>();

  [Header("Tower containers")]
  public GameObject[] t1Towers;
  public GameObject[] t2Towers;
  public GameObject[] t3Towers;
  public GameObject[] t4Towers;
  public GameObject[] t5Towers;
  public GameObject[] t6Towers;

  private void Start()
  {
    CheckTotal();
  }


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
          //tileInfo.BuildTower(RandomTower());

        }
        else
        {
          throw new System.NotImplementedException("Throw error to sceen");
        }
      }
    }
    if (Input.GetKeyDown(KeyCode.KeypadEnter))
    {
      RandomTowerType();
      CheckTotal();
    }
  }

  private int RandomTowerType()
  {
    int type = GetTowerType();

    for (int i = 0; i < gemTypeChances.Length; i++)
    {
      if (i != type)
      {
        gemTypeChances[i] += (step / (gemTypeChances.Length -1));
      }
      else
      {
        gemTypeChances[i] -= step;
      }
    }

    CheckTotal();
    return type;
  }

  private int GetTowerType()
  {
    float costSoFar = 0;
    float randNumb = Random.Range(0F, total);

    for (int i = 0; i < gemTypeChances.Length; i++)
    {
      costSoFar += gemTypeChances[i];
      if (randNumb <= costSoFar && gemTypeChances[i] != 0)
      {
        return i;
      }
    }
    return -1;
  }

  private void CheckTotal()
  {
    total = 0;
    for (int i = 0; i < gemTypeChances.Length; i++)
    {
      total += gemTypeChances[i];
    }

    if (total > 100)
    {
      gemTypeChances[Random.Range(0, 8)] -= (total - 100);
    }
  }

  //private float CfromP(decimal p)
  //{
  //  cUppper = p;
  //  cLower = 0m;
  //  p2 = 1m;

  //  while (true)
  //  {
  //    cMid = (cUppper + cLower) / 2m;
  //    p1 = PfromC(cMid);
  //    if (Mathf.Abs((float)(p1 - p2)) <= 0F) break;

  //    if (p1 > p)
  //    {
  //      cUppper = cMid;
  //    }
  //    else
  //    {
  //      cLower = cMid;
  //    }

  //    p2 = p1;
  //  }

  //  return (float)cMid;
  //}

  //private decimal PfromC(decimal C)
  //{
  //  decimal pProcOnN = 0m;
  //  decimal pProckByN = 0m;
  //  decimal sumNpProcOnN = 0m;

  //  int maxFails = (int)Mathf.Ceil((float)(1m / C));

  //  for (int N = 1; N <= maxFails; ++N)
  //  {
  //    pProcOnN = (decimal)Mathf.Min(1F, (float)(N * C)) * (1m - pProckByN);

  //  }

  //}
}
