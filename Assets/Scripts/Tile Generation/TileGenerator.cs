using System;
using System.Text;
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
  public int numberOfConnections;

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
    numberOfConnections = connections.Count;

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
    DateTime before = DateTime.Now;

    GameObject lastRow = null;
    GameObject currentRow = null;

    for (int i = 0; i < mapHeight; i++)
    {
      currentRow = new GameObject("Row: " + i);
      currentRow.transform.parent = this.transform;

      for (int j = 0; j < mapWidth; j++)
      {
        GameObject tile = MakeNewTile(i, j, currentRow.transform);
        MakeConnectionsOld(tile.GetComponent<TileInformation>());
        //MakeConnectionsByRowThenNode(tile.GetComponent<TileInformation>(), currentRow, lastRow);
        //MakeConnectionsByNode(tile.GetComponent<TileInformation>());
      }

      lastRow = currentRow;
    }

    DateTime after = DateTime.Now;

    TimeSpan duration = after.Subtract(before);

    Debug.Log("Duration of build board in miliseconds: " + duration.Milliseconds);
  
  }

  public GameObject MakeNewTile(int xPos, int zPos, Transform rowContainer)
  {
    GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, yPosition, zPos), tilePrefab.transform.rotation, rowContainer);
    tile.transform.name = "Tile: " + xPos + "-" + zPos;
    TileInformation tileInfo = tile.GetComponent<TileInformation>();
    tileInfo.myNode = ScriptableObject.CreateInstance<TileNode>();
    tileInfo.myNode.TileNodeInit(tile.transform, (xPos + "-" + zPos));
    allNodes.Insert(0, tileInfo.myNode);
    return tile;
  }

  public void MakeConnectionsByRowThenNode(TileInformation tileInfo, GameObject currentRow, GameObject previousRow)
  {
    int xPos = (int)tileInfo.transform.position.x;
    int zPos = (int)tileInfo.transform.position.z;

    // if this isnt the first tile on the row we can get the one behind it
    if (zPos > 0)
    {
      // find the node
      TileNode thisRowNode = currentRow.transform.Find("Tile: " + xPos + "-" + (zPos - 1)).GetComponent<TileInformation>().myNode;

      // add connection to the two node
      tileInfo.myNode.AddConnection(thisRowNode, true);
      thisRowNode.AddConnection(tileInfo.myNode, true);

      // add connection to gizmo
      connections.Add(new GizmoConnections(tileInfo.myNode, thisRowNode, true));
    }

    // if this isnt in the first row try to connect to the row behind it
    if (xPos > 0)
    {
      // find the node
      TileNode prevRowNode = previousRow.transform.Find("Tile: " + (xPos - 1) + "-" + zPos).GetComponent<TileInformation>().myNode;

      // add connection to the two node
      tileInfo.myNode.AddConnection(prevRowNode, true);
      prevRowNode.AddConnection(tileInfo.myNode, true);

      // add connection to gizmo
      connections.Add(new GizmoConnections(tileInfo.myNode, prevRowNode, true));
    }
  }

  public void MakeConnectionsByNode(TileInformation tileInfo)
  {
    int xPos = (int)tileInfo.transform.position.x;
    int zPos = (int)tileInfo.transform.position.z;
    
    // if this isnt the first tile on the row we can get the one behind it
    if (zPos > 0)
    {
      // find the node
      TileNode thisRowNode = transform.Find("Tile: " + xPos + "-" + (zPos - 1)).GetComponent<TileInformation>().myNode;

      // add connection to the two node
      tileInfo.myNode.AddConnection(thisRowNode, true);
      thisRowNode.AddConnection(tileInfo.myNode, true);

      // add connection to gizmo
      connections.Add(new GizmoConnections(tileInfo.myNode, thisRowNode, true));
    }

    if (xPos > 0)
    {
      // find the node
      TileNode prevRowNode = transform.Find("Tile: " + (xPos - 1) + "-" + zPos).GetComponent<TileInformation>().myNode;

      // add connection to the two node
      tileInfo.myNode.AddConnection(prevRowNode, true);
      prevRowNode.AddConnection(tileInfo.myNode, true);

      // add connection to gizmo
      connections.Add(new GizmoConnections(tileInfo.myNode, prevRowNode, true));
    }
  }

  public void MakeConnectionsOld(TileInformation tileInfo)
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

        //GizmoConnections newConnection = ScriptableObject.CreateInstance<GizmoConnections>();
        //newConnection.GizmoConnectionsInit(baseNode, crossNode, true);
        //connections.Add(newConnection);

        connections.Add(new GizmoConnections(baseNode, crossNode, true));

        if (connectionsSoFar == 2)
        {
          return;
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

  public void RebuildConnections()
  {
    connections = new List<GizmoConnections>();

    foreach (TileNode node in allNodes)
    {
      for (int i = 0; i < node.nodeConnections.Length; i++)
      {
        if (node.nodeConnections[i] != null)
        {
          if (node.nodeConnections[i].node.nodeTransform.position == node.nodeTransform.position + Vector3.forward ||
              node.nodeConnections[i].node.nodeTransform.position == node.nodeTransform.position + Vector3.right)
          {
            Debug.Log("Making new connection between " + node.name + " & " + node.nodeConnections[i].node.name);

            //GizmoConnections newConnection = ScriptableObject.CreateInstance<GizmoConnections>();
            //newConnection.GizmoConnectionsInit(node, node.nodeConnections[i].node, node.nodeConnections[i].IsConnected);
            //connections.Add(newConnection);
            connections.Add(new GizmoConnections(node, node.nodeConnections[i].node, node.nodeConnections[i].IsConnected));
          }
        }
      }
    }
  }

  public void Clear()
  {
    while (transform.childCount > 0)
    {
      foreach (Transform child in transform)
      {
        DestroyImmediate(child.gameObject);
      }
    }

    allNodes = new List<TileNode>();

    connections = new List<GizmoConnections>();
  }

  public void ClearGizmoConnections()
  {
    connections = new List<GizmoConnections>();
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
}

[Serializable]
public class GizmoConnections
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

  public void GizmoConnectionsInit(TileNode nodeOne, TileNode nodeTwo, bool connected)
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