using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSow : MonoBehaviour
{
    // Start is called before the first frame update
    public static SelectedSow Instance;
    public SowData Data;
    public Image BaseImage;
    public Image[] Buttons;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
   
    public void BuyCrops()
    {
        if (CashManager.Instance.Money >= Data.SowPrice9x)
        {
            PlayerSowAbility.Instance.SowMethod(Data.SowType, 9);
            if (SowSeason.Instance != null)
                SowSeason.Instance.SowSeasonChanged(SeasonManager.Instance.SeasonStateMain);
            CashManager.Instance.AddCoins(-Data.SowPrice9x);
        }
    }
}
