using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerAnim))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerHarvestAbility : MonoBehaviour
{


    public static PlayerHarvestAbility Instance;

    [Header("Elements")]
    [SerializeField] private Transform _harvestSphere;
    private PlayerAnim _playerAnimator;
    private PlayerToolSelector _playerToolSelector;

    [Header("Settings")]
    public CropField CurrentCropField;
    public bool _canHarvest;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        _playerAnimator = GetComponent<PlayerAnim>();
        _playerToolSelector = GetComponent<PlayerToolSelector>();

      //  WaterParticles.OnWaterCollided += WaterCollidedCallBack;
        //  CropField.onFullySown += CropFieldFullySownCallBack;
        CropField.onFullyHarvested += CropFieldFullyHarvestedCallBack;
        _playerToolSelector._selectedToolAction += ToolSelectedCallBack;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanHarvest())
        {
            _playerAnimator.StopHarvestAnimation();
            _playerAnimator.StopCuttingAnimation();
        }
    }
    private void OnDestroy()
    {
        //WaterParticles.OnWaterCollided -= WaterCollidedCallBack;
        // CropField.onFullySown -= CropFieldFullySownCallBack;
        _playerToolSelector._selectedToolAction -= ToolSelectedCallBack;
        CropField.onFullyHarvested -= CropFieldFullyHarvestedCallBack;
    }
    private void CropFieldFullyHarvestedCallBack(CropField cropField)
    {
        if (cropField == CurrentCropField && CurrentCropField.CropType != CropType.Tree)
            _playerAnimator.StopHarvestAnimation();

        if (cropField == CurrentCropField && CurrentCropField.CropType == CropType.Tree)
            _playerAnimator.StopCuttingAnimation();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") &&CurrentCropField!=null)
        {
            CurrentCropField = other.GetComponent<CropField>();
          //  EnteredCropField(_currentCropField);
            //burada child objede olsaydý getcomponentInparent olucaktý
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            if (CurrentCropField != null)
            {
                _playerAnimator.StopHarvestAnimation();
                _playerAnimator.StopCuttingAnimation();
            }
            CurrentCropField = null;
        }
    }
    private void EnteredCropField(CropField cropField)
    {
        
        if (_playerToolSelector.CanHarvest())
        {
            if (CurrentCropField == null)
                CurrentCropField = cropField;
                if (cropField.CropType != CropType.Tree)
                {
                    _playerAnimator.PlayHarvestAnimation();
                    if (_canHarvest)
                        CurrentCropField.Harvest(_harvestSphere);
                }
                if (cropField.CropType == CropType.Tree)
                {
                    _playerAnimator.PlayCuttingAnimation();
                    if (_canHarvest && cropField.TreeHarvestAmount == cropField.TreeHarvestIncrease)
                    {
                        CurrentCropField.Harvest(_harvestSphere);
                        cropField.TreeHarvestIncrease = 0;
                      CurrentCropField = null;

                    }
                }
                
            }
        _canHarvest = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField"))
            EnteredCropField(other.GetComponent<CropField>());
    }
    public void HarvestingStartedCallBack()
    {
        _canHarvest = true;
    }
    public void HarvestingStoppedCallBack()
    {
        _canHarvest = false;
    }
}

