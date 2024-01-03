using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedType : MonoBehaviour
{
    public SowData SowDatas;
    
    public void AddSow()
    {
        PlayerSowAbility.Instance.SowMethod(SowDatas.SowType,10);
        if(SowSeason.Instance!=null)
        SowSeason.Instance.SowSeasonChanged(SeasonManager.Instance.SeasonStateMain);
    }
}
