#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileGenerator))]
public class TileGeneratorEditor : Editor
{
  public override void OnInspectorGUI()
  {
    TileGenerator tileGen = (TileGenerator)target;
    DrawDefaultInspector();

    GUILayout.Space(10);
    if (GUILayout.Button("Build TileGrid"))
    {
      tileGen.Clear();
      tileGen.BuildTileGrid();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Clear information"))
    {
      tileGen.Clear();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Rebuild gizmo connections"))
    {
      tileGen.RebuildConnections();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Clear gizmo connections"))
    {
      tileGen.ClearGizmoConnections();
    }
  }
}
#endif
