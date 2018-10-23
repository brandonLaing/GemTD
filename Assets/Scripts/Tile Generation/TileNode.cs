using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileNode : ScriptableObject
{
  public Transform nodeTransform;
  public NodeConnection[] nodeConnections = new NodeConnection[4];

  // constructor for new tileNode
  public void TileNodeInit(Transform transform, string nodeNumber)
  {
    this.name = "Node: " + nodeNumber;
    this.nodeTransform = transform;
  }

  // Display Connections
  public void DisplayConnections()
  {
    // Make a new string builder
    StringBuilder sb = new StringBuilder();

    // say what node your showing connections for
    sb.AppendLine("Connections for " + this.name + ":");
    // go though each connection
    for (int i = 0; i < nodeConnections.Length; i++)
    {
      // check if there is a connection in that spot
      if (nodeConnections[i] != null)
      {
        // if there is a connection display that node and if its connected
        sb.AppendLine(nodeConnections[i].node.name + ": " + nodeConnections[i].IsConnected);
      }
    }

    // output the string builder to our console 
    Debug.LogWarning(sb.ToString());
  }

  // Blocks this tile connections and connections to it
  public void Block()
  {
    // go though every connection
    for (int i = 0; i < nodeConnections.Length; i++)
    {
      if (nodeConnections[i] != null)
      {
        // set this status to that connection to false
        nodeConnections[i].IsConnected = false;
        // get that connections connection to this and set its status to false
        nodeConnections[i].node.FindConnection(this).IsConnected = false;
        // update the tile generations gizmos to reflect that the connection is now false
        TileGenerator.main.UpdateConnection(nodeConnections[i].node, nodeConnections[i].node.FindConnection(this).node, false);
      }
    }

    // send message to log that this tile is block
    Debug.Log(this.name + " was blocked");
  }

  // Unblocks this tile connection and connections to it
  public void Unblock()
  {
    // go though ever connection
    for (int i = 0; i < nodeConnections.Length; i++)
    {
      if (nodeConnections[i] != null)
      {
        // set this status to that connection to true
        nodeConnections[i].IsConnected = true;
        // get that connections connection to this and set its status to true
        nodeConnections[i].node.FindConnection(this).IsConnected = true;
        // update the tile generations gizmo to reflect that connection is now true
        TileGenerator.main.UpdateConnection(nodeConnections[i].node, nodeConnections[i].node.FindConnection(this).node, true);
      }
    }

    // send message to log that this tile is blocked
    Debug.Log(this.name + " was unblocked");
  }

  // Find a connection this nodes list of connections using a reference to a node that should be in its connections list
  public NodeConnection FindConnection(TileNode node)
  {
    // go though each connections
    for (int i = 0; i < nodeConnections.Length; i++)
    {
      // check if current connections node equals the node were looking for
      if (nodeConnections[i].node == node)
      {
        // if it is return that node back
        return nodeConnections[i];
      }
    }

    // if we don't find our correct node thats some shit so we better send an error message
    Debug.LogError("Tried to find connection for node that isn't in list of connections\n" + this.name + " and " + node.name);
    return null;
  }

  // go though ever slot and if its empty add the new node connection too it then add this node to the one we just connected too
  public void AddConnection(TileNode node, bool connectionStatus)
  {
    // the node isn't already connected
    if (!IsNodeConnected(node))
    {
      //Debug.Log("Adding connection to " + this.name);
      // go through ever position
      for (int i = 0; i < nodeConnections.Length; i++)
      {
        // if the current position is empty
        if (nodeConnections[i] == null)
        {
          // make a connection for that node in this position
          //NodeConnection connection = ScriptableObject.CreateInstance<NodeConnection>();
          //connection.NodeConnectionInit(node, connectionStatus);
          //nodeConnections[i] = connection;
          nodeConnections[i] = new NodeConnection(node, connectionStatus);
          return;
        }
      }
    }
  }

  // removes connection between this node and another then removes the other nodes connection to this node 
  public void RemoveConnection(TileNode node)
  {
    // this node has a connection to other node
    if (IsNodeConnected(node))
    {
      // find that node
      for (int i = 0; i < nodeConnections.Length; i++)
      {
        if (nodeConnections[i].node == node)
        {
          // then empty this node
          nodeConnections[i] = null;
          return;
        }
      }
    }
  }

  // check if a node is connected to this node
  public bool IsNodeConnected(TileNode node)
  {
    for (int i = 0; i < nodeConnections.Length; i++)
    {
      if (nodeConnections[i] != null)
      {
        if (nodeConnections[i].node == node)
        {
          return true;
        }
      }
    }
    return false;
  }
}

// basic data type that holds a node and its connection status
[Serializable]
public class NodeConnection
{
  public TileNode node;
  public bool IsConnected;

  public NodeConnection(TileNode node, bool IsConnected)
  {
    this.node = node;
    this.IsConnected = IsConnected;
  }

  public void NodeConnectionInit(TileNode node, bool IsConnected)
  {
    this.node = node;
    this.IsConnected = IsConnected;
  }
}
