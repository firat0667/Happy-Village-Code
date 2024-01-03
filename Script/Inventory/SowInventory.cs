using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowInventory
{
    [SerializeField] private List<SowItem> _sowItems = new List<SowItem>();
    public void SowCallBack(CropType sowType, int sowAmount)
    {
        bool SowFound = false;
        for (int i = 0; i < _sowItems.Count; i++)
        {
            SowItem item = _sowItems[i];
            if (item.SowType == sowType)
            {
                item.SowAmount = item.SowAmount + sowAmount; // rastegele lýk getýrebýlýrsýn her hasat da 5 mýsýr gibi 
                Debug.Log(_sowItems[i].SowAmount);
                SowFound = true;
                break;
            }
        }
        //DebugInventory();

        if (SowFound)
            return;
        // create a ne item in the list with that croptype
        _sowItems.Add(new SowItem(sowType, sowAmount));
    }
    public SowItem[] GetSowItems()
    {
        return _sowItems.ToArray();
    }
    public void SowClear(int sowNumber)
    {
        if (_sowItems[sowNumber] != null)
        {
            _sowItems.Remove((_sowItems[sowNumber]));
            PlayerSowAbility.Instance.CurrentSowAmount = 0;
        }

    }
}
