using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileGenerator : MonoBehaviour
{
  [Header("Map Settings")]
  public int mapHeight = 37;
  public int mapWidth = 37;
  public int yPosition = 0;

  [Header("Resources")]
  public GameObject tilePrefab;

  [Header("Lists")]
  public List<TileNode> allNodes = new List<TileNode>();
  public List<GizmoConnections> connections = new List<GizmoConnections>();

  [Header("Main Stuff")]
  public static TileGenerator main;
  public bool IsMain;

  private void Start()
  {
    if (tag == "TileGenerator")
    {
      IsMain = true;
      main = this;
    }
  }

  private void Update()
  {
    if (main == this)
    {
      IsMain = true;
    }
    else if (tag == "TileGenerator")
    {
      main = this;
    }
    else
    {
      IsMain = false;
    }
  }

  public void BuildTileGrid()
  {
    int tileNumber = 1;
    for (int i = 0; i < mapHeight; i++)
    {
      for (int j = 0; j < mapWidth; j++)
      {
        GameObject tile = MakeNewTile(i, j, tileNumber);
        MakeConnections(tile.GetComponent<TileInformation>());
        tileNumber++;
      }
    }
  }

  public GameObject MakeNewTile(int xPos, int zPos, int tileNumber)
  {
    GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, yPosition, zPos), tilePrefab.transform.rotation, this.transform);
    tile.transform.name = "Tile: " + tileNumber;
    TileInformation tileInfo = tile.GetComponent<TileInformation>();
    tileInfo.myNode = ScriptableObject.CreateInstance<TileNode>();
    tileInfo.myNode.TileNodeInit(tile.transform, tileNumber);
    allNodes.Insert(0, tileInfo.myNode);
    return tile;
  }

  public void MakeConnections(TileInformation tileInfo)
  {
    int connectionsSoFar = 0;

    TileNode baseNode = tileInfo.myNode;

    foreach (TileNode crossNode in allNodes)
    {
      if (baseNode.nodeTransform.position == crossNode.nodeTransform.position + Vector3.forward ||
          baseNode.nodeTransform.position == crossNode.nodeTransform.position + Vector3.right)
      {
        connectionsSoFar++;
        baseNode.AddConnection(crossNode, true);
        crossNode.AddConnection(baseNode, true);

        connections.Add(new GizmoConnections(baseNode, crossNode, true));
      }
    }
  }

  private void OnDrawGizmos()
  {
    if (allNodes.Count > 0 && connections.Count > 0)
    {
      Gizmos.color = Color.black;
      foreach (TileNode node in allNodes)
      {
        Gizmos.DrawSphere(node.nodeTransform.position + new Vector3(0, 0.5F, 0), 0.2F);
      }

      foreach (GizmoConnections connection in connections)
      {
        if (connection.conneted)
        {
          Gizmos.color = Color.green;
          Gizmos.DrawLine(connection.positionOne, connection.positionTwo);
        }
        else
        {
          Gizmos.color = Color.red;
          Gizmos.DrawLine(connection.positionOne, connection.positionTwo);
        }
      }
    }
  }

  public void UpdateConnection(TileNode nodeOne, TileNode nodeTwo, bool connectionState)
  {
    foreach (GizmoConnections connection in connections)
    {
      if (nodeOne == connection.startNode && nodeTwo == connection.endNode)
      {
        connection.conneted = connectionState;
      }
      else if (nodeTwo == connection.startNode && nodeOne == connection.endNode)
      {
        connection.conneted = connectionState;
      }
    }
  }

  public void Clear()
  {
    foreach (TileNode node in allNodes)
    {
      DestroyImmediate(node.nodeTransform.gameObject);
    }

    allNodes = new List<TileNode>();

    connections = new List<GizmoConnections>();
  }

}

public class GizmoConnections : ScriptableObject
{
  public Vector3 positionOne;
  public Vector3 positionTwo;
  public Vector3 direction;
  public bool conneted;

  public TileNode startNode;
  public TileNode endNode;

  public GizmoConnections(TileNode nodeOne, TileNode nodeTwo, bool connected)
  {
    this.positionOne = nodeOne.nodeTransform.position;
    this.positionTwo = nodeTwo.nodeTransform.position;

    this.positionOne.y = .5F;
    this.positionTwo.y = .5F;

    direction = this.positionOne - this.positionTwo;

    this.conneted = connected;

    startNode = nodeOne;
    endNode = nodeTwo;
  }
}