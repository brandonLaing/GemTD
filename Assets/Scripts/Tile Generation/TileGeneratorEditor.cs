﻿using System.Collections;
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
      tileGen.BuildTileGrid();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Clear information"))
    {
      tileGen.Clear();
    }
  }
}