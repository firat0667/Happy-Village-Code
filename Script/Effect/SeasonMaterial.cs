using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonMaterial : MonoBehaviour
{
    public bool Ground;
  

    private void Start()
    {
        SeasonManager.Instance.SeasonChanged += SelectMaterial;
        if (SeasonManager.Instance != null)
        {
            Debug.Log("SeasonManager.Instance is not null");
            SelectMaterial(SeasonManager.Instance.SeasonStateMain);
        }
        else
        {
            Debug.LogError("SeasonManager.Instance is null");
        }
    }
    private void OnDisable()
    {
        SeasonManager.Instance.SeasonChanged -= SelectMaterial;
    }
    public void SelectMaterial(SeasonState state)
    {
        state =SeasonManager.Instance.SeasonStateMain;
        switch (state)
        {
            case SeasonState.Spring:
                if(Ground)
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.SpringGround);
                else
                ChangeMaterialInChildren(transform,SeasonManager.Instance.SpringMaterial);
                break;
            case SeasonState.Summer:
                if (Ground)
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.SpringGround);
                else
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.SpringMaterial);
                break;
            case SeasonState.Autumn:
                if (Ground)
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.AutumnGround);
                else
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.AutumnMaterial);
                break;
            case SeasonState.Winter:
                if (Ground)
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.WinterGround);
                else
                    ChangeMaterialInChildren(transform, SeasonManager.Instance.WinterMaterial);
                break;
            default:
                throw new ArgumentException("Ge�ersiz mevsim de�eri", nameof(state));
        }
    }
   
    void ChangeMaterialInChildren(Transform parent,Material material)
    {
        // Parent nesnenin alt�ndaki t�m nesneleri d�ng�ye al
        foreach (Transform child in parent)
        {
            // Malzeme de�i�ikli�i yap
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = material;
                Debug.Log("SeasonMay" + material);
            }

            // Alt nesneleri kontrol et
            ChangeMaterialInChildren(child, material);
        }
    }
}

