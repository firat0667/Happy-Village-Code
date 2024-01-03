using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireManager : MonoBehaviour
{
    public GameObject Villager;
   public void HireLabrorer()
    {
        if (CashManager.Instance.Money >= 1000 && InventoryDisplay.Instance.IsStorageActive)
        {
            GameObject villager = Instantiate(Villager, InventoryDisplay.Instance.VillagerSpawnPos); 
            villager.GetComponent<VilalgerAI>().VillagerType = VillagerType.Labrorer;
            CashManager.Instance.AddCoins(-1000);
        }
    }
    public void HireWoodCutter()
    {
        if (CashManager.Instance.Money >= 1000 && InventoryDisplay.Instance.IsLumberjackActive)
        {
            GameObject villager = Instantiate(Villager, InventoryDisplay.Instance.VillagerSpawnPos);
            villager.GetComponent<VilalgerAI>().VillagerType = VillagerType.WoodCutter;
            CashManager.Instance.AddCoins(-1000);
        }
    }
    public void HireMiner()
    {
        if (CashManager.Instance.Money >= 1000 && InventoryDisplay.Instance.IsBlacksmithActive)
        {
            GameObject villager = Instantiate(Villager, InventoryDisplay.Instance.VillagerSpawnPos);
            villager.GetComponent<VilalgerAI>().VillagerType = VillagerType.Miner;
            CashManager.Instance.AddCoins(-1000);
        }
    }
}
