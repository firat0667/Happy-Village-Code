using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public CropType CropType;
    public int amount;
    public InventoryItem(CropType cropType, int amount)
    {
      this.CropType = cropType;
      this.amount = amount;
    }
}
