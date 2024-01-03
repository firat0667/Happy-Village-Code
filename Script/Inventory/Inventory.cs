using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    [SerializeField] private List<InventoryItem> _inventoryItems = new List<InventoryItem>();
    public List<InventoryItem> InventoryItems
    {
        get { return _inventoryItems; }
        set { _inventoryItems = value; }
    }

    public void CropHarvestedCallBack(CropType cropType,CropData cropData)
    {
       
        InventoryManager.Instance.SaveCapacity(cropData.FarmAmount);
        InventoryManager.Instance.LoadCapacity();
        bool cropFound = false;
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                InventoryItem item = _inventoryItems[i];
                if (item.CropType == cropType)
                {
                    item.amount = item.amount + cropData.FarmAmount; // rastegele lýk getýrebýlýrsýn her hasat da 5 mýsýr gibi
                cropFound = true;
                    break;
                }
            }
        //DebugInventory();
          BuildingGrid.Instance.UiPanelUpgrade();
           if (cropFound)
                return;
        _inventoryItems.Add(new InventoryItem(cropType, cropData.FarmAmount));
        // create a ne item in the list with that croptype

    }

    public InventoryItem[] GetInventoryItems()
    {
        return _inventoryItems.ToArray();
    }
    public void DebugInventory()
    {
        foreach (InventoryItem item in _inventoryItems)
            Debug.Log("We have "+item.amount+" item in our "+item.CropType+" list.");
        
    }

  

    public void Clear(int CropNumber)
    {
        if (_inventoryItems[CropNumber] != null)
        {
            _inventoryItems.Remove((_inventoryItems[CropNumber]));
            
            if (_inventoryItems.Count > 0)
            {
                InventoryDisplay.Instance.SelectedItemNumber = 0;
                InventoryDisplay.Instance.SellSlider.maxValue = _inventoryItems[0].amount;
            }
            
        }
        
    }
}
