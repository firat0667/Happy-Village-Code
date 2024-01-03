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
            _myInstiateObject = requirementObj; // Buras� gereksiz olabilir, kullanm�yorsan�z silin
            // �lgili kayna��n ikonunu y�kleyin
            iconImage.sprite = LoadResourceIcon(requirement.resourceType);

            // Kaynak miktar�n� ekleyin
            Text amountText = requirementObj.GetComponentInChildren<Text>();
            amountText.text = $"{requirement.resourceAmount}";
            // Kaynak yetersizse "Yetersiz" yaz ve panel rengini k�rm�z� yap
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
        // ResourceType'a g�re ilgili ikonu y�kleyin
        switch (resourceType)
        {
            case ResourceType.Wood:
                return Resources.Load<Sprite>("Icons/WoodIcon");
            case ResourceType.Stone:
                return Resources.Load<Sprite>("Icons/StoneIcon");
            case ResourceType.Money:
                return Resources.Load<Sprite>("Icons/MoneyIcon");
            // �ste�e ba�l� olarak di�er kaynak t�rlerini ekleyebilirsiniz
            default:
                return null;
        }
    }
   
    private void PanelColorByResourceType(Image panelImage, ResourceType resourceType,bool avaible)
    {
        // ResourceType'a g�re ilgili panel rengini ayarlay�n
        switch (resourceType)
        {
            case ResourceType.Wood:
                panelImage.color =avaible? Color.green:Color.red; // �rne�in, ah�ap i�in ye�il renk
                break;
            case ResourceType.Stone:
                panelImage.color = avaible ? Color.green : Color.red; // �rne�in, metal i�in gri renk
                break;
            case ResourceType.Money:
                panelImage.color =avaible? Color.green : Color.red; // �rne�in, para i�in sar� renk
                break;
                // �ste�e ba�l� olarak di�er kaynak t�rlerini ekleyebilirsiniz
        }
    }
    public void IsResourcesFull()
    {
        if (AreAllResourcesAvailable())
        {
            ReduceAllResources();
            // Sat�n al�nan ��enin i�lemlerini buraya ekleyebilirsiniz.
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
            // Para kayna�� i�in azaltma i�lemi (varsay�lan olarak CashManager.Instance.Money kullan�ld�)
            CashManager.Instance.AddCoins(-amount);
           
        }
        else
        {
            // Di�er kaynaklar i�in azaltma i�lemi
            for (int i = 0; i < InventoryManager.Instance.InventoryItems.Count; i++)
            {
                var inventory = InventoryManager.Instance.InventoryItems[i];
                if (inventory.CropType == ResourceTypeToCropType(resourceType))
                {
                    inventory.amount -= amount;
                    break; // Gerekli miktar kadar azalt�ld�, d�ng�den ��k
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
            // Di�er kaynaklar i�in azaltma i�lemi
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
                return CropType.Empty; // Di�er durumlar i�in belirtilen bir de�er d�nd�r�lebilir.
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
