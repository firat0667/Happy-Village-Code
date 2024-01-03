using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseMenu : MonoBehaviour
{
    private Image _itemIconImage;
    public Transform requirementsParent;
    public GameObject requirementPrefab;
    public Text NameText;
    public BuildSO CurrentSelectedItem;
    private Button _myButton;
    private GameObject _myInstiateObject;

    private void OnEnable()
    {
        BuildingGrid.OnUpdateRequirementsUI += UpdateRequirementsUI;
    }
    
    private void Start()
    {
        _myButton = GetComponent<Button>();
        _itemIconImage = GetComponent<Image>();
        SelectItem(CurrentSelectedItem);
        NameText.text = CurrentSelectedItem.itemName;
    }
    private void OnDisable()
    {
        BuildingGrid.OnUpdateRequirementsUI -= UpdateRequirementsUI;
    }

    public void SelectItem(BuildSO item)
    {
        CurrentSelectedItem = item;

        _itemIconImage.sprite = item.icon;

        UpdateRequirementsUI();
    }

    public void UpdateRequirementsUI()
    {
        foreach (Transform child in requirementsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var requirement in CurrentSelectedItem.resourceRequirements)
        {
            GameObject requirementObj = Instantiate(requirementPrefab, requirementsParent);
            Image iconImage = requirementObj.GetComponent<Image>();
            _myInstiateObject = requirementObj; // Burasý gereksiz olabilir, kullanmýyorsanýz silin
            // Ýlgili kaynaðýn ikonunu yükleyin
            iconImage.sprite = LoadResourceIcon(requirement.resourceType);

            // Kaynak miktarýný ekleyin
            Text amountText = requirementObj.GetComponentInChildren<Text>();
            amountText.text = $"{requirement.resourceAmount}";
            // Kaynak yetersizse "Yetersiz" yaz ve panel rengini kýrmýzý yap
            bool isResourceAvailable = IsResourceAvailable(requirement.resourceType, requirement.resourceAmount);
            PanelColorByResourceType(requirementObj.GetComponent<ChildPanel>().PanelImage, requirement.resourceType, isResourceAvailable);
        }
    }
    private void Update()
    {

        foreach (var requirement in CurrentSelectedItem.resourceRequirements)
        {
            bool isResourceAvailable = IsResourceAvailable(requirement.resourceType, requirement.resourceAmount);
            PanelColorByResourceType(_myInstiateObject.GetComponent<ChildPanel>().PanelImage, requirement.resourceType, isResourceAvailable);
        }

        if (AreAllResourcesAvailable())
            _myButton.enabled = true;
        else
            _myButton.enabled = false;

}

    public void PurchaseItem()
    {
        if (CurrentSelectedItem != null)
        {
            Debug.Log($"Purchased {CurrentSelectedItem.itemName}");
        }
    }

    private Sprite LoadResourceIcon(ResourceType resourceType)
    {
        // ResourceType'a göre ilgili ikonu yükleyin
        switch (resourceType)
        {
            case ResourceType.Wood:
                return Resources.Load<Sprite>("Icons/WoodIcon");
            case ResourceType.Stone:
                return Resources.Load<Sprite>("Icons/StoneIcon");
            case ResourceType.Money:
                return Resources.Load<Sprite>("Icons/MoneyIcon");
            // Ýsteðe baðlý olarak diðer kaynak türlerini ekleyebilirsiniz
            default:
                return null;
        }
    }
   
    private void PanelColorByResourceType(Image panelImage, ResourceType resourceType,bool avaible)
    {
        // ResourceType'a göre ilgili panel rengini ayarlayýn
        switch (resourceType)
        {
            case ResourceType.Wood:
                panelImage.color =avaible? Color.green:Color.red; // Örneðin, ahþap için yeþil renk
                break;
            case ResourceType.Stone:
                panelImage.color = avaible ? Color.green : Color.red; // Örneðin, metal için gri renk
                break;
            case ResourceType.Money:
                panelImage.color =avaible? Color.green : Color.red; // Örneðin, para için sarý renk
                break;
                // Ýsteðe baðlý olarak diðer kaynak türlerini ekleyebilirsiniz
        }
    }
    public void IsResourcesFull()
    {
        if (AreAllResourcesAvailable())
        {
            ReduceAllResources();
            // Satýn alýnan öðenin iþlemlerini buraya ekleyebilirsiniz.
            PurchaseItem();
        }
        BuildingGrid.Instance.UiPanelUpgrade();

    }
    private void ReduceAllResources()
    {
        foreach (var requirement in CurrentSelectedItem.resourceRequirements)
        {
            ReduceResource(requirement.resourceType, requirement.resourceAmount);
        }
    }

    private void ReduceResource(ResourceType resourceType, int amount)
    {
        if (resourceType == ResourceType.Money)
        {
            // Para kaynaðý için azaltma iþlemi (varsayýlan olarak CashManager.Instance.Money kullanýldý)
            CashManager.Instance.AddCoins(-amount);
           
        }
        else
        {
            // Diðer kaynaklar için azaltma iþlemi
            for (int i = 0; i < InventoryManager.Instance.InventoryItems.Count; i++)
            {
                var inventory = InventoryManager.Instance.InventoryItems[i];
                if (inventory.CropType == ResourceTypeToCropType(resourceType))
                {
                    inventory.amount -= amount;
                    break; // Gerekli miktar kadar azaltýldý, döngüden çýk
                }
            }
        }
    }
    private bool IsResourceAvailable(ResourceType resourceType, int requiredAmount)
    {
        if (resourceType == ResourceType.Money)
        {
            return (requiredAmount <= CashManager.Instance.Money);
        }
        else
        {
            // Diðer kaynaklar için azaltma iþlemi
            int availableAmount = 0;

            for (int i = 0; i < InventoryManager.Instance.InventoryItems.Count; i++)
            {
                var inventory = InventoryManager.Instance.InventoryItems[i];
                if (inventory.CropType == ResourceTypeToCropType(resourceType))
                {
                    availableAmount += inventory.amount;
                }
            }

            return (requiredAmount <= availableAmount);
        }
    }
    private CropType ResourceTypeToCropType(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Wood:
                return CropType.Tree;
            case ResourceType.Stone:
                return CropType.Rock;
            default:
                return CropType.Empty; // Diðer durumlar için belirtilen bir deðer döndürülebilir.
        }
    }
    public bool CroptypeMethod (CropType cropType,int buildResourcesAmount)
    {
        
        for (int i = 0; i < InventoryManager.Instance.InventoryItems.Count; i++)
        {
            var inventory = InventoryManager.Instance.InventoryItems[i];
            if (inventory.CropType == cropType)
            {
                if (buildResourcesAmount <= inventory.amount)
                    return true;   
            }
        }
        return false;
    }

    public bool AreAllResourcesAvailable()
    {
        foreach (var requirement in CurrentSelectedItem.resourceRequirements)
        {
            if (!IsResourceAvailable(requirement.resourceType, requirement.resourceAmount))
            {
                return false;
            }
        }
        return true;
    }
}
