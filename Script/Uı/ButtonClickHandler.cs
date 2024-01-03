using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
  
    void Start()
    {
        // Bu örnekte, butonun týklama olayýna abone oluyoruz
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Buton týklandýðýnda kendi GameObject'ini hedef GameObject'e atýyoruz
        
          BuildingGrid.Instance.ButtonObject=gameObject;
    }
}
