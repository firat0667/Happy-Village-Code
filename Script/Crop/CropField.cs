using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CropField : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Elements")]
    [SerializeField] private Transform tilesParents;
    public Transform CropParent;
    public List<CropTile> _cropTiles= new List<CropTile>();
    public bool IsHarvested = false;

    [Header("Settings")]
    public CropData CropData;
    private int _tilesSown;
    private int _tilesWatered;
    private int _tilesHarvested;
    private int _treeEnableCount;
    public CropType CropType;
    public int TreeHarvestAmount;
    public int TreeHarvestIncrease;

    [Header("SeedDATAS")]
    public CropData CornData;
    public CropData TomatoData;
    public CropData EggPlantData;
    public CropData PumpkinData;
    public CropData TurnipData;
    public CropData CarrotData;
    public CropData PotatoData;
    public CropData PepperData;
    public CropData WheatData;

    public Transform ChildRenderer;

    [HideInInspector] public TileFieldState State;
    [Header("Actions")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvested;
    private Collider _collider;
    

    void Start()
    {
        _collider = GetComponent<Collider>();
        if (CropType != CropType.Tree)
        {
            StoreTiles();
            State = TileFieldState.Empty;
        }
       
    }
    private void OnEnable()
    {
        if (CropType == CropType.Tree&&CropParent.transform.childCount<=0)
        {
            StoreTiles();
            for (int x = 0; x < _cropTiles.Count; x++)
            {
                Sow(_cropTiles[x]);
                Water(_cropTiles[x]);
            }
            //  cropField.State = TileFieldState.Watered;
        }
        
    }
    private void OnDisable()
    {
        if (CropType == CropType.Tree)
        {
            LeanTween.cancel(ChildRenderer.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space))
            InstantlySowTiles();
        // bu method test asamasý ýcýn tumm tohumlarý bý tusla ekiyor
        */
        if (CropType == CropType.Tree)
        {
            if (IsHarvested)
            {
                gameObject.SetActive(false);
                // StartCoroutine(SpawnTrees());
                IsHarvested = false;
            }
            if(ChildRenderer == null || (ChildRenderer!=null&& ChildRenderer.localScale.x < 1f))
            {
                _collider.enabled = false;
            }
            else if(ChildRenderer != null && ChildRenderer.localScale.x >= 1f)
            {

                _collider.enabled = true;
                if (!VillagerAiDestinations.Instance.TreeTransform.Contains(transform))
                {
                    VillagerAiDestinations.Instance.TreeTransform.Add(transform);
                }
            }
        }
        if (State== TileFieldState.Sow)
        FieldFullyWatered();
        if (CropType != CropType.Tree && PlayerSowAbility.Instance.CurrentSowData != CropType.Empty)
        {
            CropDataType(PlayerSowAbility.Instance.CurrentSowData);
        }
    }
    CropData CropDataType(CropType type) => (type)switch
    {
        (CropType.Corn) => CropData = CornData,
        (CropType.Tomato) => CropData = TomatoData,
        (CropType.EggPlant) => CropData = EggPlantData,
        (CropType.Pumpkin) => CropData = PumpkinData,
        (CropType.Carrot) => CropData = CarrotData,
        (CropType.Turnip) => CropData = TurnipData,
        (CropType.Potato) => CropData = PotatoData,
        (CropType.Pepper) => CropData = PepperData,
        (CropType.Wheat) => CropData = WheatData,

    };
    public void StoreTiles()
    {
        for (int i = 0; i < tilesParents.childCount; i++)
        {
            if(!_cropTiles.Contains(tilesParents.GetChild(i).GetComponent<CropTile>()))
            _cropTiles.Add(tilesParents.GetChild(i).GetComponent<CropTile>());
        }
          
       
    }
    public void SeedCollidedCallback(Vector3[] seedPositions)
    {
        for (int i = 0; i <seedPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(seedPositions[i]);
            if (closestCropTile == null)
                continue;
            if (!closestCropTile.IsEmpty())
                continue;
            // eger ustteki þart saðlanýyorsa continue yani sýradaki  i ye geç 0 sa 1 olucak
               Sow(closestCropTile);
        } 
    }

    public void Sow(CropTile cropTile)
    {
        cropTile.Sow(CropData);
        _tilesSown++;
        if(PlayerSowAbility.Instance.CurrentSowAmount>0)
        PlayerSowAbility.Instance.SowMethod(CropData.CropType,-1);
        InventoryManager.Instance.ClearInventory();
        if (_tilesSown == _cropTiles.Count)
            FieldFullySown();
    }
    public void WaterCollidedCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(waterPositions[i]);
            if (closestCropTile == null)
                continue;
            if (!closestCropTile.IsSown())
                continue;
            Water(closestCropTile);

        }
    }
    public void Water(CropTile cropTile)
    {
        cropTile.Water();
        _tilesWatered++;
        cropTile.IsWatered();
        if (_tilesWatered == _cropTiles.Count)
            FieldFullyWatered();
    }
    private void FieldFullySown()
    {
        Debug.Log("Field Fully Sown");
        State = TileFieldState.Sow;
        onFullySown?.Invoke(this);
       
    }
    private void FieldFullyWatered()
    {
        bool isFieldFullyWatered = true; // Varsayýlan olarak tüm alanýn sulandýðý kabul edilsin

        for (int i = 0; i < _cropTiles.Count; i++)
        {
            if (_cropTiles[i].GetComponent<CropTile>().CropParent.childCount > 0 && CropType!=CropType.Tree)
            {
                float cropSizeX = _cropTiles[i].GetComponent<CropTile>().CropParent.GetChild(0).GetComponent<Crop>().CropRenderer.localScale.x;

                if (cropSizeX < 1)
                {
                    isFieldFullyWatered = false;
                    break; // En az bir aðaç yeterince büyük deðilse döngüyü sonlandýrýn
                }
            }
            else if(CropType == CropType.Tree)
            {
                float cropSizeX = _cropTiles[i].GetComponent<CropTile>().CropParent.GetChild(0).GetComponent<Crop>().CropRenderer.localScale.x;

                if (cropSizeX < 1)
                {
                    isFieldFullyWatered = false;
                    break; // En az bir aðaç yeterince büyük deðilse döngüyü sonlandýrýn
                }
            }
        }

        if (isFieldFullyWatered)
        {
            Debug.Log("Field Fully Watered");
            State = TileFieldState.Watered;
            onFullyWatered?.Invoke(this);
        }

    }
    public void Harvest(Transform harvestSphere)
    {
        float sphereRadius = harvestSphere.localScale.x;
        for (int i = 0; i < _cropTiles.Count; i++)
        {
            if (_cropTiles[i].IsEmpty() && CropType!=CropType.Tree)
            continue;
            float distanceCropShere = Vector3.Distance(harvestSphere.position, _cropTiles[i].transform.position);
            if (distanceCropShere < sphereRadius)
                HarvestTile(_cropTiles[i]);
            IsHarvested = true;
        }
    }
    private void HarvestTile(CropTile cropTile)
    {
        cropTile.Harvest();
        _tilesHarvested++;
        if (_tilesHarvested == _cropTiles.Count)
            FieldFullyHarvested();

    }
    private void FieldFullyHarvested()
    {
        _tilesSown = 0;
        _tilesWatered = 0;
        _tilesHarvested = 0;

        State = TileFieldState.Empty;
        onFullyHarvested?.Invoke(this);
    }
    // bu buton o tusa bastýgýmýzda dýrek tarlayý dolduruyor 
    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        for (int i = 0; i < _cropTiles.Count; i++)
        {
            Sow(_cropTiles[i]);
        }
    }
    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        for (int i = 0; i < _cropTiles.Count; i++)
        {
            Water(_cropTiles[i]);
        }
    }
    private CropTile GetClosestCropTile(Vector3 seedPositions)
    {
        float minDistance = 5000;
        int closestCropTileIndex = -1;
        for (int i = 0; i <_cropTiles.Count; i++)
        {
            CropTile cropTile = _cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position,seedPositions);

            if(distanceTileSeed < minDistance)
            {
                minDistance = distanceTileSeed;
                closestCropTileIndex = i;
            }
        }
        if(closestCropTileIndex == -1)
            return null;

        return _cropTiles[closestCropTileIndex];
    }
    public bool IsAnyTileEmpty()
    {
        foreach (CropTile tile in _cropTiles)
        {
            if (tile.IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAllTilesSown()
    {
        foreach (CropTile tile in _cropTiles)
        {
            if (!tile.IsSown())
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAllTilesWatered()
    {
        foreach (CropTile tile in _cropTiles)
        {
            if (!tile.IsWatered())
            {
                return false;
            }
        }
        return true;
    }
    public bool IsEmpty()
    {
        return State == TileFieldState.Empty;
    }
    public bool IsSown()
    {
        return State == TileFieldState.Sow;
    }
    public bool IsWatered()
    {
        return State == TileFieldState.Watered;
    }

}
