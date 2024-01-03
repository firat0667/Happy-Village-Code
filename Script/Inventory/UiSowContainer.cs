using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSowContainer : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Elements")]
    [SerializeField] private Image _sowIconImage;
    [SerializeField] private TextMeshProUGUI _sowamountText;
    public CropType CropType;
    public SowData SowData;

    private void Awake()
    {
        for (int i = 0; i < DataManagers.instance.SowDatas.Length; i++)
        {
            if (DataManagers.instance.SowDatas[i].SowType == CropType)
                SowData = DataManagers.instance.SowDatas[i];
        }
    }
    public void Configure(Sprite icon, int amount ,CropType cropType)
    {
        _sowIconImage.sprite = icon;
        _sowamountText.text = amount.ToString();
        CropType = cropType;
    }
    private void Update()
    {
        if(CropType==PlayerSowAbility.Instance.CurrentSowData)
    PlayerSowAbility.Instance.CurrentSowAmount = int.Parse(_sowamountText.text);
    }

    public void UpdateDisplay(int amount)
    {
        _sowamountText.text = amount.ToString();
    }
    public void SelectSeed()
    {
        PlayerSowAbility.Instance.CurrentSowData = CropType;
    }
}
