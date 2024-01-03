using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class SeasonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public event Action<SeasonState> SeasonChanged;
    public static SeasonManager Instance;
    public SeasonState SeasonStateMain;
    private int _currentSeason;
    private int _currentYear = 1;
    [SerializeField] private float _basetimer = 30f;
    private float _timer = 600f;

    [Header("UI")]
    public Text YearText;
    public Text TimerText;
    public Slider SeasonSlider;
    public Image SliderImage;

    [Header("Light Settings")]
    public GameObject DirectionelLight;
    private float _minAngle=25;
    private float _maxAngle = 180;

    [Header("SeasonColor")]
    public Color SpringColor;
    public Color SummerColor;
    public Color AutumnColor;
    public Color WinterColor;

    [Header("SeasonMaterial")]
    public Material SpringMaterial; // Yeni malzeme
    public Material AutumnMaterial;
    public Material WinterMaterial;
    [Header("SeasonMaterialGround")]
    public Material SpringGround; // Yeni malzeme
    public Material AutumnGround;
    public Material WinterGround;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
    }
    private void Start()
    {
        LoadSeason();
        SeasonSlider.minValue = 0;
        SeasonSlider.maxValue = _basetimer;
        SeasonType(_currentSeason);
        SeasonColor(_currentSeason);
        SliderImage.color = YearText.color;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            SaveSeason();
        }
        else
        {
            if (_currentSeason < 3)
            {
                
                _currentSeason += 1;
            }
                
            else
            {
                _currentSeason = 0;
                _currentYear += 1;
            }

            _timer = _basetimer;
            SeasonType(_currentSeason);
            SeasonColor(_currentSeason);
            SeasonChanged?.Invoke(SeasonStateMain);
            SliderImage.color = YearText.color;
            SaveSeason();
        }
        SeasonSlider.value = _timer;
        UpdateUIText();
        DirectionalLightRotation();
    }
    public void DeleteSeason()
    {
        _currentYear = 1;
        _currentSeason = 0;
        SaveSeason();
    }
    private void DirectionalLightRotation()
    {
        float t = Mathf.Clamp01(_timer / _basetimer); // 0 ile 1 arasýna sýnýrla
        float targetAngle = Mathf.Lerp(_maxAngle, _minAngle, t); // Belirtilen açý aralýðýnda lineer bir geçiþ yap
        DirectionelLight.transform.rotation = Quaternion.Euler(targetAngle, 0f, 0f);
    }

    public SeasonState SeasonType(int season)
    {
      
        return season switch
        {
            0 => SeasonStateMain = SeasonState.Spring,
            1 => SeasonStateMain = SeasonState.Summer,
            2 => SeasonStateMain = SeasonState.Autumn,
            3 => SeasonStateMain = SeasonState.Winter,
            _ => throw new ArgumentException("Geçersiz mevsim deðeri", nameof(season))

        };
    }
    public Color SeasonColor(int season)
    {
        return season switch
        {
            0 => YearText.color = SpringColor,
            1 => YearText.color = SummerColor,
            2 => YearText.color = AutumnColor,
            3 => YearText.color = WinterColor,
            _ => throw new ArgumentException("Geçersiz mevsim deðeri", nameof(season)),
        };
    }
   
    
    public void SaveSeason()
    {
        PlayerPrefs.SetInt("_currentSeason", _currentSeason);
        PlayerPrefs.SetInt("_currentYear", _currentYear);
        PlayerPrefs.SetFloat("_timer", _timer);
        PlayerPrefs.Save(); // Save
    }
    public void LoadSeason()
    {
      _currentSeason= PlayerPrefs.GetInt("_currentSeason", _currentSeason);
     _currentYear  = PlayerPrefs.GetInt("_currentYear", _currentYear);
     _timer  = PlayerPrefs.GetFloat("_timer", _timer);

    }
    private void UpdateUIText()
    {
        // Dakika ve saniyeyi hesapla
        int minutes = Mathf.FloorToInt(_timer / 60);
        int seconds = Mathf.FloorToInt(_timer % 60);

        // TimerText'i güncelle
        if(seconds >= 0)
        TimerText.text = $"{minutes:D2}:{seconds:D2}";
        YearText.text = SeasonStateMain.ToString()+" "+ _currentYear.ToString();
    }
}
