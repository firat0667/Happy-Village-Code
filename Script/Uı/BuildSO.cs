using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildSO", menuName = "Scriptable Objects/BuildSO", order = 2)]
public class BuildSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ResourceRequirement[] resourceRequirements;
}

[System.Serializable]
public class ResourceRequirement
{
    public ResourceType resourceType;
    public int resourceAmount;
}
public enum ResourceType
{
    Money,
    Wood,
    Stone,
    // Buraya istediðiniz diðer kaynak türlerini ekleyebilirsiniz.
}
