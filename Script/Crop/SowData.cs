using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sow Data", menuName = "Scriptable Objects/Sow Data", order = 1)]

public class SowData : ScriptableObject
{
    [Header("Settings")]
   
    public CropType SowType;
    public Sprite SowIcon;
    public int SowPrice9x;
    public SeasonState SeasonState;
}
