using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileInformation))]
public class TileInformationEditor : Editor
{
  public override void OnInspectorGUI()
  {
    TileInformation tileInfo = (TileInformation)target;
    DrawDefaultInspector();

    GUILayout.Space(10);
    if (GUILayout.Button("Display Connections"))
    {
      tileInfo.myNode.DisplayConnections();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Block Tile"))
    {
      tileInfo.myNode.Block();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Unblock Tile"))
    {
      tileInfo.myNode.Unblock();
    }
  }
}
