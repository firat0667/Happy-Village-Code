using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCropContainer : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Elements")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _backGroundImage;
    public Button SellButton;
    public Color ButtonColor;
    public int SelectedResources;
    private void Start()
    {
       if(transform.parent.name!= "SellDisplay")
      SellButton.gameObject.SetActive(false);
        else
        {
            _iconImage.gameObject.SetActive(false);
            GetComponent<Image>().enabled = false;
            _backGroundImage.gameObject.SetActive(false);
        }
            
    }
    public void SelectedCrop()
    {
       
    }
    public void Configure(Sprite icon,int amount)
    {
        _iconImage.sprite = icon;
        _amountText.text = amount.ToString();
    }
    public void SellConfigure(Sprite icon, int amount)
    {
        SellButton.image.sprite = icon;
        _amountText.text = amount.ToString();
    }
    public void UpdateDisplay(int amount) 
    { 
        _amountText.text = amount.ToString();
    }
    public void SelectSellCrop()
    {
     Transform  parentTransform = transform.parent;

        if (parentTransform != null)
        {
            // Parent'ýn child sayýsýný al
            int childCount = parentTransform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                // Her child objeyi kontrol et
                Transform childTransform = parentTransform.GetChild(i);

                // Eðer bu child obje benim olduðum objeye eþitse, kaçýncý child olduðunu yazdýr
                if (childTransform == transform)
                {
                    Debug.Log("Bu obje " + i + ". child obje.");
                    InventoryDisplay.Instance.SelectedItemNumber = i;
                    SellButton.image.color = ButtonColor;
                    // break; Bulduktan sonra döngüyü durdurabilirsiniz. 
                }

                else if(childTransform != transform)
                {
                    childTransform.GetComponent<UiCropContainer>().SellButton.image.color = Color.white;
                }
            }
        }
        else
        {
            Debug.Log("Bu objenin bir parenti yok.");
        }
        InventoryDisplay.Instance.Slider();
    }
}
