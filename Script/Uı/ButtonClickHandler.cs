using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
  
    void Start()
    {
        // Bu �rnekte, butonun t�klama olay�na abone oluyoruz
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Buton t�kland���nda kendi GameObject'ini hedef GameObject'e at�yoruz
        
          BuildingGrid.Instance.ButtonObject=gameObject;
    }
}
