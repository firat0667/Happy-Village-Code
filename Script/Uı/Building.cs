using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class Building : MonoBehaviour
{
    public Vector2Int Size;
    public float Ypos;
    public Transform customPivot; // Assign the custom pivot point in the Inspector.
    public Vector3 FirstPos;
    public bool IsGrounded;
    public bool IsField;
    public bool IsStorage;
    public bool IsLumberJack;
    public bool IsBlackSmith;
    public bool IsAvailable
    {
        get; set;
    }
    private Crop _crop;
    public Transform Center;
    private void Start()
    {
        if(transform.parent != null)
        FirstPos = transform.position;
        _crop = GetComponentInChildren<Crop>();
    }
    private void OnEnable()
    {
        if(transform.parent == null)
        {
            Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();

            // Se�ili objenin t�m child collider'lar�n� ge�ici olarak devre d��� b�rak�n.
            foreach (Collider childCollider in childColliders)
            {
                childCollider.enabled = false;
            }
        }
        if(IsBlackSmith)
            InventoryDisplay.Instance.IsBlacksmithActive = true;
        if(IsLumberJack)
            InventoryDisplay.Instance.IsLumberjackActive = true;
        if (IsStorage)
            InventoryDisplay.Instance.IsLumberjackActive = true;

    }
    private void Update()
    {
        
        ColliderControll(Center,Size.x/2,Size.y/2);
        if (BuildingGrid.Instance.isBuildingComplete && !IsAvailable)
        {
            transform.position = FirstPos;
         //   SetAllChildrenNormal();
            if (_crop != null && !_crop.IsBuild)
            {
                _crop.ScaleUpdate();
                _crop.IsBuild = true;
            }
           
        }
    }
    public void ColliderControll(Transform myPosition, float x, float y)
    {
        Collider[] colliders = Physics.OverlapBox(myPosition.position, new Vector3(x, 5, y));

        foreach (Collider collider in colliders)
        {
            if (!collider.CompareTag("Ground"))
            {
                // E�er "Ground" etiketi olmayan bir collider ile �arp���yorsak, ge�erlilik false d�nd�r�l�r.
                IsAvailable = false;
                break;
            }
            else
                IsAvailable = true;

        }
    }
    public void SetAllChildrenTransparent()
    {
       
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>(true); // T�m alt �ocuklarda Renderer bile�enlerini al
        Color colorToSet = IsAvailable ? Color.green : Color.red;

        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.material.color = colorToSet;
        }
    }
    

    public void SetAllChildrenNormal()
    {
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>(true);
        if (IsField)
        {
            CropTile[] cropTiles = GetComponentsInChildren<CropTile>(true);
            foreach (CropTile cropTile in cropTiles)
            {
                if(cropTile.State==TileFieldState.Watered)
                cropTile.State = TileFieldState.Sow;
            }
        }
        foreach (Renderer childRenderer in childRenderers)
            {
                // E�er "croptile" script'i yoksa, rengi de�i�tir
                childRenderer.material.color = Color.white;
            }

            if (!BuildingGrid.Instance.isBuildingComplete)
            {
                BuildingGrid.Instance.BuildPrefab = this;
                BuildingGrid.Instance.BuildRotatePanel.SetActive(true);
                BuildingGrid.Instance.TurnAround = customPivot;
            }

            Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();

            // Se�ili objenin t�m child collider'lar�n� ge�ici olarak devre d��� b�rak�n.
            foreach (Collider childCollider in childColliders)
            {
                childCollider.enabled = true;
            }
        // FirstPos'u burada s�f�rlay�n, kontrollerden sonra
      //  FirstPos = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                Gizmos.color = new Color(0f,1f,0f,0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x,0,y),new Vector3(1,1f,1));
            }
        }
    }
}
