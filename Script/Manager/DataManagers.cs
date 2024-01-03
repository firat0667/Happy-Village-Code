using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManagers : MonoBehaviour
{
    public static DataManagers instance;
    [Header("Data")]
    [SerializeField] private CropData[] _cropDatas;
    public SowData[] SowDatas;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        instance = this;
        else
            Destroy(gameObject);
    }
    public Sprite GetCropSpriteFromCropType(CropType cropType)
    {
        for (int i = 0; i < _cropDatas.Length; i++)
            if (_cropDatas[i].CropType == cropType)
                return _cropDatas[i].Icon;
        Debug.LogError("No cropData found of that crop");
        return null;
        
    }
    public Sprite GetSowpriteFromCropType(CropType cropType)
    {
        for (int i = 0; i < SowDatas.Length; i++)
            if (SowDatas[i].SowType == cropType)
                return SowDatas[i].SowIcon;
        Debug.LogError("No cropData found of that crop");
        return null;

    }
    public int GetCropPriceFromCroptype(CropType cropType)
    {
        for (int i = 0; i < _cropDatas.Length; i++)
            if (_cropDatas[i].CropType == cropType)
                return _cropDatas[i].Price;
        Debug.LogError("No cropData found of that crop");
        return 0;
    }
}
