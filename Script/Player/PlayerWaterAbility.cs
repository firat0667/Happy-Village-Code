using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnim))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerWaterAbility : MonoBehaviour
{
    [Header("Elements")]
    private PlayerAnim _playerAnimator;
    private PlayerToolSelector _playerToolSelector;

    [Header("Settings")]
    private CropField _currentCropField;


    // Start is called before the first frame update
    void Start()
    {
        
        _playerAnimator = GetComponent<PlayerAnim>();
        _playerToolSelector = GetComponent<PlayerToolSelector>();

        WaterParticles.OnWaterCollided += WaterCollidedCallBack;
        //  CropField.onFullySown += CropFieldFullySownCallBack;
           CropField.onFullyWatered += CropFieldFullyWaterCallBack;
        _playerToolSelector._selectedToolAction += ToolSelectedCallBack;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanWater())
            _playerAnimator.StopWaterAnimation();
    }
    private void OnDestroy()
    {
        WaterParticles.OnWaterCollided -= WaterCollidedCallBack;
       // CropField.onFullySown -= CropFieldFullySownCallBack;
        _playerToolSelector._selectedToolAction -= ToolSelectedCallBack;
          CropField.onFullyWatered-=CropFieldFullyWaterCallBack;
    }
    private void CropFieldFullyWaterCallBack(CropField cropField)
    {
        if (cropField == _currentCropField)
            _playerAnimator.StopWaterAnimation();
    }
    private void WaterCollidedCallBack(Vector3[] waterPositions)
    {
        if (_currentCropField == null)
            return;
        _currentCropField.WaterCollidedCallback(waterPositions);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            _currentCropField = other.GetComponent<CropField>();
            EnteredCropField(_currentCropField);
            //burada child objede olsaydý getcomponentInparent olucaktý
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            _playerAnimator.StopWaterAnimation();
            _currentCropField = null;
        }
    }
    private void EnteredCropField(CropField cropField)
    {
        if (_playerToolSelector.CanWater())
        {
            if(_currentCropField == null)
            _currentCropField = cropField;
            _playerAnimator.PlayWaterAnimation();
        }
           
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField"))
            EnteredCropField(other.GetComponent<CropField>());
    }
}
