using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildData
{
    public string BuildingType;
  [HideInInspector]  public Vector3 BuildPos;
  [HideInInspector]  public Quaternion BuildDir;
    public List<Vector3> BuildPosList;
    public List<Quaternion> BuildDirList;
    public List<string> BuildDirNames;
    public BuildData()
    {
        BuildPosList = new List<Vector3>();
        BuildDirList = new List<Quaternion>();
        BuildDirNames = new List<string>();
    }
}
