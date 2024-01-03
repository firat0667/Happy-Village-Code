using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _cropContainerParent;
    [SerializeField] private Transform _sowContainerParent;
    public Transform SalesContainerParent;
    [SerializeField] private UiCropContainer _uiCropContainerPref;
    [SerializeField] private UiSowContainer _uiSowContainerPref;
    [SerializeField] private DataManagers _dataManagers;
    [Header("SellingDisplay")]
    public GameObject SellParent;
    public GameObject BuySow;
    public GameObject LabrorerPanel;
    public GameObject MinerPanel;
    public GameObject WoodCutterPanel;
    public static InventoryDisplay Instance;
    public Slider SellSlider;
    public TextMeshProUGUI SliderText;
    public int SelectedItemNumber;
    [Header("Capacity")]
    public Slider CapacitySlider;
    public Image CapacitySliderImage;
    public GameObject FullCapacity;
    [Header("Hire")]
    public bool IsStorageActive;
    public bool IsBlacksmithActive;
    public bool IsLumberjackActive;
    public GameObject LumberJackText;
    public GameObject MinerText;
    public GameObject LabrorerText;
    public Transform VillagerSpawnPos;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        SellParent.SetActive(false);
    }
    private void Update()
    {
        SliderText.text = ((int)SellSlider.value).ToString();
        CapacitySlider.maxValue = InventoryManager.Instance.MaxCapacity;
        CapacitySlider.minValue = 0;

        // Baþlangýç deðerini ayarla
        CapacitySlider.value = InventoryManager.Instance.Capacity;

        // Slider'ýn deðerini güncelle
        UpdateSliderColor(InventoryManager.Instance.Capacity);

        if(IsStorageActive)
            LabrorerText.SetActive(false);
        else LabrorerText.SetActive(true);
        if(IsLumberjackActive)
            LumberJackText.SetActive(false);
        else LumberJackText.SetActive(true);
        if (IsBlacksmithActive)
            MinerText.SetActive(false);
        else MinerText.SetActive(true);

    }


    public void Slider()
    {
        Inventory inventory = InventoryManager.Instance.GetInventory();
        InventoryItem[] ýnventoryItems = inventory.GetInventoryItems();
        SellSlider.minValue = 0;
        if (ýnventoryItems[SelectedItemNumber]!=null)
        SellSlider.maxValue = ýnventoryItems[SelectedItemNumber].amount;
    }
    private void UpdateSliderColor(float value)
    {
        // Max capacity'nin %30'una göre renk belirle
        if (value <= InventoryManager.Instance.MaxCapacity * 0.3f)
        {
            CapacitySliderImage.color = Color.green;
        }
        // %30-60 arasý
        else if (value <= InventoryManager.Instance.MaxCapacity * 0.6f)
        {
            CapacitySliderImage.color = Color.yellow;
        }
        // %60-90 arasý
        else if (value <= InventoryManager.Instance.MaxCapacity * 0.9f)
        {
            CapacitySliderImage.color = new Color(1f, 0.5f, 0f); // Turuncu renk
        }
        // %90'dan büyük
        else
        {
            CapacitySliderImage.color = Color.red;
        }

        if(value > InventoryManager.Instance.MaxCapacity * 0.99f)
        {
            FullCapacity.gameObject.SetActive(true);
        }
        else
            FullCapacity.gameObject.SetActive(false);
    }

        public void Configure(Inventory inventory)
    {
        InventoryItem[] ýnventoryItems=inventory.GetInventoryItems();
        for (int i = 0; i < ýnventoryItems.Length; i++)
        {
            UiCropContainer uýCropContainer = Instantiate(_uiCropContainerPref, _cropContainerParent);
            Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(ýnventoryItems[i].CropType);
            uýCropContainer.Configure(cropIcon,ýnventoryItems[i].amount);
        }
    }
    public void SowConfigure(SowInventory inventory)
    {
        SowItem[] ýnventoryItems = inventory.GetSowItems();
        for (int i = 0; i < ýnventoryItems.Length; i++)
        {
                UiSowContainer uýSowContainer = Instantiate(_uiSowContainerPref, _sowContainerParent);
            Sprite cropIcon = DataManagers.instance.GetSowpriteFromCropType(ýnventoryItems[i].SowType);
            uýSowContainer.Configure(cropIcon, ýnventoryItems[i].SowAmount, ýnventoryItems[i].SowType);
        }
    }
    public void SalesConfigure(Inventory inventory)
    {
        InventoryItem[] ýnventoryItems = inventory.GetInventoryItems();
        for (int i = 0; i < ýnventoryItems.Length; i++)
        {
            UiCropContainer uýCropContainer = Instantiate(_uiCropContainerPref, SalesContainerParent);
            Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(ýnventoryItems[i].CropType);
            uýCropContainer.SellConfigure(cropIcon, ýnventoryItems[i].amount);
        }
    }
    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items= inventory.GetInventoryItems();
        UiCropContainer cropContainer;
        for (int i = 0; i <items.Length; i++)
        {
            if (i < _cropContainerParent.childCount)
            {
                cropContainer=_cropContainerParent.GetChild(i).GetComponent<UiCropContainer>();
                cropContainer.gameObject.SetActive(true);
            }
            else
                cropContainer = Instantiate(_uiCropContainerPref, _cropContainerParent);
            Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(items[i].CropType);
            cropContainer.Configure(cropIcon, items[i].amount);
            cropContainer.SellConfigure(cropIcon, items[i].amount);
        }
    
        int remaningContainers = _cropContainerParent.childCount - items.Length;
        if (remaningContainers <= 0)
            return;
        for (int i = 0; i < remaningContainers; i++)
       _cropContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
         
        
        /*
        // clear crop container parents if  there any ui crop containers
        while (cropContainerParent.childCount>0)
        {
          Transform container= cropContainerParent.GetChild(0);
          container.SetParent(null);
          Destroy(container.gameObject);
        }
        Configure(inventory);
      
        for (int i = 0; i < items.Length; i++)
        {
            UýCropContainer uýCropContainer = Instantiate(_uiCropContainerPref, cropContainerParent);
            Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(items[i].CropType);
            uýCropContainer.Configure(cropIcon, items[i].amount);
        }
        */
    }
    public void UpdateSalesDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();
        UiCropContainer cropContainer;
        for (int i = 0; i < items.Length; i++)
        {
            if (i < SalesContainerParent.childCount)
            {
                cropContainer = SalesContainerParent.GetChild(i).GetComponent<UiCropContainer>();
                cropContainer.gameObject.SetActive(true);
            }
            else
             cropContainer = Instantiate(_uiCropContainerPref, SalesContainerParent);
            Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(items[i].CropType);
            cropContainer.Configure(cropIcon, items[i].amount);
            cropContainer.SellConfigure(cropIcon, items[i].amount);
        }

        int remaningContainers = SalesContainerParent.childCount - items.Length;
        if (remaningContainers <= 0)
            return;
        for (int i = 0; i < remaningContainers; i++)
            SalesContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
        
    }
    public void UpdateSowDisplay(SowInventory inventory)
    {
        SowItem[] items = inventory.GetSowItems();
        UiSowContainer sowContainer;
        for (int i = 0; i < items.Length; i++)
        {
            if (i < _sowContainerParent.childCount)
            {
                sowContainer = _sowContainerParent.GetChild(i).GetComponent<UiSowContainer>();
               // sowContainer.gameObject.SetActive(true);
            }
            else
                sowContainer = Instantiate(_uiSowContainerPref, _sowContainerParent);
            Sprite cropIcon = DataManagers.instance.GetSowpriteFromCropType(items[i].SowType);
            sowContainer.Configure(cropIcon, items[i].SowAmount, items[i].SowType);
        }
        int remaningContainers = _sowContainerParent.childCount - items.Length;
        if (remaningContainers <= 0)
            return;
        for (int i = 0; i < remaningContainers; i++)
            _sowContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
        /*
        // clear crop container parents if  there any ui crop containers
        while (cropContainerParent.childCount>0)
        {
          Transform container= cropContainerParent.GetChild(0);
          container.SetParent(null);
          Destroy(container.gameObject);
        }
        Configure(inventory);
      
        for (int i = 0; i < items.Length; i++)
        {
            UýCropContainer uýCropContainer = Instantiate(_uiCropContainerPref, cropContainerParent);
            Sprite cropIcon = DataManagers.instance.GetCropSpriteFromCropType(items[i].CropType);
            uýCropContainer.Configure(cropIcon, items[i].amount);
        }
        */
    }

}
