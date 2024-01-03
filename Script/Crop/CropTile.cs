using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Threading;


// dýþarýya cýkardýgýmýz ýcýn   heryerden erýsebýlýyoruz
public class CropTile : MonoBehaviour
{
    public TileFieldState State;
    // Start is called before the first frame update
    [Header("Elements")]
     public Transform CropParent;
    [SerializeField] private MeshRenderer _tileRenderer;
    [SerializeField] private CropField _cropFieldParent;
  [SerializeField]  private Crop _crop;
   [SerializeField]private CropData _cropData;
    [Header("Events")]
    public static Action<CropType,CropData> onCropHarvested;
    [Header("Save")]
   [SerializeField] private int _plantTypeInt;
    private string _olcekAnahtar;
    private string _boolHarvested;
    private string _uniqueName;
    private static int _uniqueNameCounter = 0;
    private int _isharvested;
    public CropData[] Cropdatas;
  [SerializeField]  private float _waterTimer;
    private float _timer;
    private bool _playFirstTime;

    void OnEnable()
    {
        if (_cropFieldParent.CropType != CropType.Tree)
        {
            State = TileFieldState.Empty;
         //   _tileRenderer.gameObject.LeanColor(Color.white * .3f, 1).setEase(LeanTweenType.easeOutBack);
            // CropField parent'inin kaçýncý child olduðunu kullanarak benzersiz bir anahtar oluþtur
            // int cropFieldIndex = _cropFieldParent.transform.GetSiblingIndex();
            _uniqueName = "CropTile_" + _uniqueNameCounter.ToString();
            _uniqueNameCounter++;
            _olcekAnahtar = GenerateUniqueKey(_uniqueName);
            _boolHarvested=GenerateUniqueKeyBool(_uniqueName);
            GetUniqueName();
            _plantTypeInt = PlayerPrefs.GetInt(_olcekAnahtar);
            _isharvested = PlayerPrefs.GetInt(_boolHarvested);
            if (_isharvested == 1)
            {
                _crop = Instantiate(Cropdatas[_plantTypeInt].CropPrefab, CropParent);
                _cropData = Cropdatas[_plantTypeInt];
                _cropFieldParent.CropType = _cropData.CropType;
                State = TileFieldState.Sow;
                _waterTimer = (_cropData.GrownTime / 3);
                _timer = _waterTimer;

            }
        }
        
    }
    string GenerateUniqueKey(string uniqueName)
    {
        return "SavePlant_" + uniqueName;
    }
    string GenerateUniqueKeyBool(string uniqueName)
    {
        return "Harvested" + uniqueName;
    }
    public string GetUniqueName()
    {
        return _uniqueName;
    }
    // Update is called once per frame
    void Update()
    {
        if(State == TileFieldState.Watered && _cropFieldParent.CropType != CropType.Tree) 
        {
            _waterTimer -= Time.deltaTime;
            if(_waterTimer<=0 &&_crop.CropRenderer.localScale.x<1f)
            {
                _waterTimer=0;
                _crop.StopGrowthAnimation();
                State=TileFieldState.Sow;
                if (!_playFirstTime)
                {
                    TileColorChangeToBase();
                    _playFirstTime = true;
                }

            }
        }

    }
    void OnApplicationQuit()
    {
        // Uygulama kapatýldýðýnda kaydet
        if(_cropData!=null &&_cropData.CropType != CropType.Tree)
        SavePlantType();
    }
    public void TileColorChangeToBase()
    {
        LeanTween.cancel(_tileRenderer.gameObject);
        _tileRenderer.gameObject.LeanColor(Color.white, 1).setEase(LeanTweenType.easeOutBack);
        Debug.Log("CropTile Awake - LeanTween hata ayýklama etkinleþtirildi.");
    }
    int PlantTipe (CropType type) => (type) switch
    {
        (CropType.Corn) => _plantTypeInt = 0,
        (CropType.Tomato) => _plantTypeInt = 1,
        (CropType.EggPlant) => _plantTypeInt = 2,
        (CropType.Pumpkin) => _plantTypeInt = 3,
        (CropType.Carrot) => _plantTypeInt = 4,
        (CropType.Turnip) => _plantTypeInt = 5,
        (CropType.Potato) => _plantTypeInt = 6,
        (CropType.Pepper) => _plantTypeInt = 7,
        (CropType.Wheat) => _plantTypeInt = 8,

    };
    public void SavePlantType()
    {
        if(_cropData.CropType!=CropType.Tree)
        _plantTypeInt = PlantTipe(_cropData.CropType);
        PlayerPrefs.SetInt(_olcekAnahtar, _plantTypeInt);
        PlayerPrefs.SetInt(_boolHarvested, _isharvested);
    }
    public void Sow(CropData cropData)
    {
        State= TileFieldState.Sow;
        Debug.Log(State);
        //  GameObject go=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //  go.transform.position = transform.position;
        //  go.transform.localScale = Vector3.one / 2;
        _crop = Instantiate(cropData.CropPrefab, transform.position, Quaternion.identity,CropParent);
        this._cropData = cropData;
        _isharvested = 1;
        SavePlantType();
        _waterTimer = (_cropData.GrownTime / 3);
        _timer = _waterTimer;
    }
    public void Water()
    {

        State = TileFieldState.Watered;
        _waterTimer = _timer;
        _playFirstTime = false;
        // _tileRenderer.material.color= Color.white*.3f;//(1,1,1,1)
        Debug.Log(State);
        _crop.ScaleUp();
        // bu code alttaki kodlarý saglýyor
        LeanTween.cancel(_tileRenderer.gameObject);
        _tileRenderer.gameObject.LeanColor(Color.white * .3f, 1).setEase(LeanTweenType.easeOutBack);
       //  StartCoroutine(ColorileCoroutine());
        if(_cropFieldParent.CropType!=CropType.Tree)
        _crop.ScaleSaver.Kaydet();
    }
    public void Harvest()
    {
        if (State == TileFieldState.Watered&&_crop.CropRenderer.gameObject.transform.localScale.x>=1)
        {
            _tileRenderer.gameObject.LeanColor(Color.white, 1).setEase(LeanTweenType.easeOutBack);
            State = TileFieldState.Empty;
            _crop.ScaleDown();
            onCropHarvested?.Invoke(_cropData.CropType, _cropData);
            _isharvested = 0;
            SavePlantType();
            PlayerPrefs.DeleteKey(_olcekAnahtar);
            PlayerPrefs.Save();
        }

    }
    public void TreeHarvestingAi()
    {
        onCropHarvested?.Invoke(_cropData.CropType, _cropData);
    }
    /*
    IEnumerator ColorileCoroutine()
    {
        float duration = 1;
        float timer = 0;
        while (timer < duration)
        {
            float t = timer / duration;
            Color lerpedColor = Color.Lerp(Color.white,Color.white*.3f,t);

            _tileRenderer.material.color = lerpedColor;
            timer += Time.deltaTime;
            yield return null;

        }
        yield return null;
    }
    */
    public bool IsEmpty()
    {
        return State == TileFieldState.Empty;
    }
    public bool IsSown()
    {
        return State == TileFieldState.Sow;
    }
    public bool IsWatered()
    {
        return State==TileFieldState.Watered;
    }
}
