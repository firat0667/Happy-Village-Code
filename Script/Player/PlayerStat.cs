using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Elements")]
   [SerializeField] private int _fishingAbility;
   [SerializeField] private int _farmingAbility;
   [SerializeField] private int _cuttingAbility;
   [SerializeField] private int _miningAbility;

    // FishingAbility için get ve set yöntemleri
    public int FishingAbility
    {
        get { return _fishingAbility; }
        set
        {
            _fishingAbility = value;
            PlayerPrefs.SetInt("FishingAbility", value);
        }
    }

    // FarmingAbility için get ve set yöntemleri
    public int FarmingAbility
    {
        get { return _farmingAbility; }
        set
        {
            _farmingAbility = value;
            PlayerPrefs.SetInt("FarmingAbility", value);
        }
    }

    // CuttingAbility için get ve set yöntemleri
    public int CuttingAbility
    {
        get { return _cuttingAbility; }
        set
        {
            _cuttingAbility = value;
            PlayerPrefs.SetInt("CuttingAbility", value);
        }
    }

    // MiningAbility için get ve set yöntemleri
    public int MiningAbility
    {
        get { return _miningAbility; }
        set
        {
            _miningAbility = value;
            PlayerPrefs.SetInt("MiningAbility", value);
        }
    }

    private void Start()
    {
        LoadPlayerData();
    }

    // Oyuncu verilerini yükleme iþlevi
    public void LoadPlayerData()
    {
        _fishingAbility = PlayerPrefs.GetInt("FishingAbility", _fishingAbility);
        _farmingAbility = PlayerPrefs.GetInt("FarmingAbility",_farmingAbility);
        _cuttingAbility = PlayerPrefs.GetInt("CuttingAbility", _cuttingAbility);
        _miningAbility = PlayerPrefs.GetInt("MiningAbility",_miningAbility);
        Debug.Log(_fishingAbility + _farmingAbility + _cuttingAbility + _miningAbility);
    }

    // Diðer oyun mekaniði ve iþlevleri burada devam eder...

}
