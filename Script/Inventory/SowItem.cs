using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SowItem
{
    public CropType SowType;
    public int SowAmount;
    public SowItem(CropType sowType, int sowamount)
    {
        this.SowType = sowType;
        this.SowAmount = sowamount;

    }

   
}
