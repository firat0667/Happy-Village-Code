using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Crop Data",menuName ="Scriptable Objects/Crop Data",order =0)]
public class CropData : ScriptableObject
{
    [Header("Settings")]
    public Crop CropPrefab;
    public CropType CropType;
    public Sprite Icon;
    public int Price;
    public float GrownTime;
    public int FarmAmount;
    public int RawFruitTime;
}
