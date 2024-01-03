using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyerIntrector : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private InventoryManager _inventoryManager;
    [HideInInspector] public Inventory Inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            Inventory inventory = InventoryManager.Instance.GetInventory();
            InventoryItem[] �nventoryItems = inventory.GetInventoryItems();
            if (�nventoryItems.Length > 0)
            {
                // SellCrops();
                InventoryDisplay.Instance.UpdateSalesDisplay(InventoryManager.Instance.Inventory);
                InventoryDisplay.Instance.SellParent.SetActive(true);
                InventoryDisplay.Instance.SellSlider.maxValue = �nventoryItems[InventoryDisplay.Instance.SelectedItemNumber].amount;
            }

        }
        if(other.CompareTag("BuySow"))
            InventoryDisplay.Instance.BuySow.SetActive(true);
        if (other.CompareTag("Labrorer"))
            InventoryDisplay.Instance.LabrorerPanel.SetActive(true);
        if (other.CompareTag("Miner"))
            InventoryDisplay.Instance.MinerPanel.SetActive(true);
        if (other.CompareTag("WoodCutter"))
            InventoryDisplay.Instance.WoodCutterPanel.SetActive(true);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            Inventory inventory = InventoryManager.Instance.GetInventory();
            InventoryItem[] �nventoryItems = inventory.GetInventoryItems();
            if (�nventoryItems.Length <= 0)
            {
                // SellCrops();
                InventoryDisplay.Instance.SellParent.SetActive(false);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            InventoryDisplay.Instance.SellParent.SetActive(false);
            BuildingGrid.Instance.UiPanelUpgrade();
        }
        if (other.CompareTag("BuySow"))
            InventoryDisplay.Instance.BuySow.SetActive(false);
        if (other.CompareTag("Labrorer"))
            InventoryDisplay.Instance.LabrorerPanel.SetActive(false);
        if (other.CompareTag("Miner"))
            InventoryDisplay.Instance.MinerPanel.SetActive(false);
        if (other.CompareTag("WoodCutter"))
            InventoryDisplay.Instance.WoodCutterPanel.SetActive(false);
    }
    public void SellCrops()
    {
        Inventory inventory = _inventoryManager.GetInventory();
        InventoryItem[] �nventoryItems= inventory.GetInventoryItems();
        //  burada slider olucak amount u oyle bel�rl�ycez
        int InventroyItemValue = InventoryDisplay.Instance.SelectedItemNumber;
        int SellAmount = (int)InventoryDisplay.Instance.SellSlider.value;
        int coinsEarned = 0;

            // calculate the earnings
            int itemPrice = DataManagers.instance.GetCropPriceFromCroptype(�nventoryItems[InventroyItemValue].CropType);
            coinsEarned += itemPrice * SellAmount;
      
        InventoryManager.Instance.SellCrop(InventroyItemValue, SellAmount);
            InventoryManager.Instance.SaveCapacity(-SellAmount);
            InventoryManager.Instance.LoadCapacity();

        TransactionEffectManager.Instance.PlayCoinParticles(coinsEarned);
            // give coins to the player
            Debug.Log("We've earned " + coinsEarned + " coins");
        //  CashManager.Instance.AddCoins(coinsEarned);
        // clear the inventory
        // burada sorun var d�zelt
        InventoryManager.Instance.ClearInventory();
        InventoryDisplay.Instance.SellSlider.maxValue = �nventoryItems[InventoryDisplay.Instance.SelectedItemNumber].amount;

    }
    
}
