using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnim))]
[RequireComponent (typeof(PlayerToolSelector))]
public class PlayerSowAbility : MonoBehaviour
{
    public static PlayerSowAbility Instance;
    [Header("Elements")]
    private PlayerAnim _playerAnimator;
    private PlayerToolSelector _playerToolSelector;

    [Header("Settings")]
    private CropField _currentCropField;
    public CropType CurrentSowData;

    [Header("Action")]
    public static Action<CropType, int> onSow;

    public int CurrentSowAmount;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator=GetComponent<PlayerAnim>();
        _playerToolSelector=GetComponent<PlayerToolSelector>();
        
        SeedPaticles.OnSeedsCollided += SeedsCollidedCallBack;
        CropField.onFullySown += CropFieldFullySownCallBack;

        _playerToolSelector._selectedToolAction += ToolSelectedCallBack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SowMethod(CropType cropType, int amount)
    {
        onSow?.Invoke(cropType, amount);
    }
    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanSow())
            _playerAnimator.StopSowAnimation();
    }
    private void OnDestroy()
    {
        SeedPaticles.OnSeedsCollided -= SeedsCollidedCallBack;
        CropField.onFullySown -= CropFieldFullySownCallBack;
        _playerToolSelector._selectedToolAction -= ToolSelectedCallBack;
    }
    private void CropFieldFullySownCallBack(CropField cropField)
    {
        if (cropField == _currentCropField)
            _playerAnimator.StopSowAnimation();
    }
    private void SeedsCollidedCallBack(Vector3[] positions)
    {
        if(_currentCropField != null)
        _currentCropField.SeedCollidedCallback(positions);
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
            _playerAnimator.StopSowAnimation();
            _currentCropField = null;
        }
    }
    private void EnteredCropField(CropField cropField)
    {
       
        if (CurrentSowAmount>0 && CurrentSowData!=CropType.Empty)
        {
            if (_currentCropField == null)
                _currentCropField = cropField;
            if (_playerToolSelector.CanSow())
                _playerAnimator.PlaySowAnimation();
        }
        else
            _playerAnimator.StopSowAnimation();
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField"))
            EnteredCropField(other.GetComponent<CropField>());
    }

    internal void SowMethod(object sowType, int v)
    {
        throw new NotImplementedException();
    }
}
