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
      StringBuilder sb = new StringBuilder();
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
    foreach (TileNode node in allNodes)
    {
      DestroyImmediate(node.nodeTransform.gameObject);
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

[SerializeField]
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