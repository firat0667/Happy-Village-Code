using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static InventoryManager Instance;
 [HideInInspector]public Inventory Inventory;
    [HideInInspector] public SowInventory SowInventory;
    private InventoryDisplay _display;
    private string _dataPath;
    private string _sowdataPath;
    public int Capacity = 0;
    public int MaxCapacity = 250;
   private string capacityKey = "Capacity";
    private string maxcapacityKey = "MaxCapacity";
    private string firstTimePlay = "firstTimePlay";
    private int _isFirstTime = 0;
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
    public SowData Sowdata;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        // Android platformunda kayýt yolu
       _dataPath = Path.Combine(Application.persistentDataPath, "inventoryData.txt");
       _sowdataPath = Path.Combine(Application.persistentDataPath, "SowData.txt");

        // _inventory=new Inventory();
        LoadInventory();
        LoadSowInventory();
        ConfigureInvetoryDisplay();
        ConfigureSowDisplay();
        Crop.onCropHarvested += CropHarvestedCallBack;
        CropTile.onCropHarvested += CropHarvestedCallBack;
        RockScript.onCropHarvested += CropHarvestedCallBack;
        PlayerLootingAbility.onCropHarvested += CropHarvestedCallBack;
        PlayerSowAbility.onSow += SowCallBack;
        LoadCapacity();
        GetFirsttime();
        if (_isFirstTime==0)
        {
            PlayerSowAbility.Instance.SowMethod(Sowdata.SowType, 9);
            SetFirsttime();
        }

    }

 public  void SaveCapacity(int amount)
    {
        Capacity = Capacity + amount;
        PlayerPrefs.SetInt(capacityKey, Capacity);
        PlayerPrefs.SetInt(maxcapacityKey, MaxCapacity);
        if (Capacity <= 0)
            Capacity = 0;
        if (Capacity >= MaxCapacity)
            Capacity = MaxCapacity;
        Debug.Log("SaveCapacity called. Capacity: " + Capacity);
        LoadCapacity();


    }
    public void LoadCapacity()
    {
        Capacity = PlayerPrefs.GetInt(capacityKey, Capacity);
        MaxCapacity = PlayerPrefs.GetInt(maxcapacityKey, MaxCapacity);
        Debug.Log("LoadCapacity. Capacity: " + Capacity + ", MaxCapacity: " + MaxCapacity);
    }
    void SetFirsttime()
    {
        _isFirstTime = 1;
        PlayerPrefs.SetInt(firstTimePlay, _isFirstTime);
        
    }
    void GetFirsttime()
    {
        PlayerPrefs.GetInt(firstTimePlay,_isFirstTime);
        _isFirstTime= PlayerPrefs.GetInt(firstTimePlay, _isFirstTime);
    }

    [NaughtyAttributes.Button]
    public void DeleteSaveData()
    {
        // Dosyalarý silmeden önce yazma iþlemlerini tamamlamak için SaveInventory ve SaveSowInventory çaðýrýlabilir.
        SaveInventory();
        SaveSowInventory();

        Debug.Log("SaveInventory ve SaveSowInventory iþlemleri tamamlandý.");

        // Dosyalarý silme iþlemi
        if (File.Exists(_dataPath))
        {
            Debug.Log("inventoryData dosyasý var, silmeye baþlanýyor.");
            using (StreamWriter writer = new StreamWriter(_dataPath))
            {
                writer.Close();
            }

            File.Delete(_dataPath);
            Debug.Log("inventoryData dosyasý silindi.");
        }
        else
        {
            Debug.Log("inventoryData dosyasý bulunamadý.");
        }

        if (File.Exists(_sowdataPath))
        {
            Debug.Log("SowData dosyasý var, silmeye baþlanýyor.");
            using (StreamWriter writer = new StreamWriter(_sowdataPath))
            {
                writer.Close();
            }

            File.Delete(_sowdataPath);
            Debug.Log("SowData dosyasý silindi.");
        }
        else
        {
            Debug.Log("SowData dosyasý bulunamadý.");
        }

        Debug.Log("Diðer iþlemler tamamlandý.");

        // Diðer iþlemler
        Capacity = 0;
        PlayerPrefs.SetInt(capacityKey, Capacity);
        LoadCapacity();

    }
    private void ZeroCropDeleted()
    {
        InventoryItem[] items = Inventory.GetInventoryItems();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].amount <= 0)
                Inventory.Clear(i);
        }
    }
    private void ZeroSowDeleted()
    {
        SowItem[] items = SowInventory.GetSowItems();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].SowAmount <= 0)
                SowInventory.SowClear(i);
        }
    }

    private void OnDestroy()
    {
        Crop.onCropHarvested -= CropHarvestedCallBack;
        CropTile.onCropHarvested -= CropHarvestedCallBack;
        RockScript.onCropHarvested -= CropHarvestedCallBack;
        PlayerLootingAbility.onCropHarvested -= CropHarvestedCallBack;
        PlayerSowAbility.onSow -= SowCallBack;
    }
    private void ConfigureInvetoryDisplay()
    {
        _display = GetComponent<InventoryDisplay>();
        _display.Configure(Inventory);
        _display.SalesConfigure(Inventory);
    }
    private void ConfigureSowDisplay()
    {
        _display = GetComponent<InventoryDisplay>();
        _display.SowConfigure(SowInventory);
    }

    private void CropHarvestedCallBack(CropType cropType,CropData cropData)
    {
        // update inventory
        if (Capacity < MaxCapacity)
        {
            Inventory.CropHarvestedCallBack(cropType, cropData);
            _display.UpdateDisplay(Inventory);
            SaveInventory();
        }
       
    }
    private void SowCallBack(CropType cropType,int sownAmount)
    {
        // update inventory
        SowInventory.SowCallBack(cropType,sownAmount);
       _display.UpdateSowDisplay(SowInventory);
        ZeroSowDeleted();
        SaveSowInventory();
    }

    public void ClearInventory()
    {
        _display.UpdateDisplay(Inventory);
        _display.UpdateSalesDisplay(Inventory);
        _display.UpdateSowDisplay(SowInventory);
        ZeroCropDeleted();
        SaveInventory();
        LoadInventory();
        LoadSowInventory();
        SaveSowInventory();
        // Inventory.Clear(cropnumber);
    }
    private void LoadInventory()
    {
        if (File.Exists(_dataPath))
        {
            string data = File.ReadAllText(_dataPath);
            Inventory = JsonUtility.FromJson<Inventory>(data);

            if (Inventory == null)
                Inventory = new Inventory();
        }
        else
        {
            // Dosyayý oluþturup içine boþ bir JSON objesi yazmak
            File.WriteAllText(_dataPath, JsonUtility.ToJson(new Inventory()));
            Inventory = new Inventory();
        }
        InventoryItems = Inventory.InventoryItems;
    }
    private void LoadSowInventory()
    {
        if (File.Exists(_sowdataPath))
        {
            string data = File.ReadAllText(_sowdataPath);
            SowInventory = JsonUtility.FromJson<SowInventory>(data);

            if (SowInventory == null)
                SowInventory = new SowInventory();
        }
        else
        {
            // Dosyayý oluþturup içine boþ bir JSON objesi yazmak
            File.WriteAllText(_sowdataPath, JsonUtility.ToJson(new SowInventory()));
            SowInventory = new SowInventory();
        }
    }
    public void SellCrop(int InventoryNumber, int Cropamount)
    {
        InventoryItem[] ýnventoryItems = Inventory.GetInventoryItems();
        InventoryItem item = ýnventoryItems[InventoryNumber];
        item.amount = item.amount - Cropamount;
        if (item.amount <= 0)
            ClearInventory();
    }
    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(Inventory, true);
        try
        {
            using (StreamWriter writer = new StreamWriter(_dataPath))
            {
                writer.Write(data);
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("SaveInventory Error: " + ex.Message);
        }

    }
    private void SaveSowInventory()
    {
        string data = JsonUtility.ToJson(SowInventory, true);
        try
        {
            using (StreamWriter writer = new StreamWriter(_sowdataPath))
            {
                writer.Write(data);
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("SaveSowInventory Error: " + ex.Message);
        }
    }

    public Inventory  GetInventory()
    {
        return Inventory;
    }
    public SowInventory GetSowInventory()
    {
        return SowInventory;
    }
}
