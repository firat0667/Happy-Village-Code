using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerAnim))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerLootingAbility : MonoBehaviour
{
    public static PlayerLootingAbility Instance;
    [Header("Elements")]
    private PlayerAnim _playerAnimator;
    private PlayerToolSelector _playerToolSelector;
    public GameObject LootObject;
    public GameObject DigObject;
    private bool _isLooting;
    private bool _isDigIn;
    public static Action<CropType, CropData> onCropHarvested;
    [Header("Fishing")]
    [SerializeField] private CropData _fishdata;
    public GameObject FishParent;
    public bool FishingBoolean;
    public Image FishImageLane;
    public Sprite FishErrorImage;
    public Sprite FishCorrectImage;
    public ParticleSystem FishParticle;
    public GameObject FishingSphere;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _playerAnimator = GetComponent<PlayerAnim>();
        _playerToolSelector = GetComponent<PlayerToolSelector>();

        //  WaterParticles.OnWaterCollided += WaterCollidedCallBack;
        //  CropField.onFullySown += CropFieldFullySownCallBack;
        _playerToolSelector._selectedToolAction += ToolSelectedCallBack;
    }
    private void OnDestroy()
    {
        //WaterParticles.OnWaterCollided -= WaterCollidedCallBack;
        // CropField.onFullySown -= CropFieldFullySownCallBack;
        _playerToolSelector._selectedToolAction -= ToolSelectedCallBack;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void FishLooting()
    {
        onCropHarvested?.Invoke(CropType.Fish,_fishdata);
    }
    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanNone())
        {
            _playerAnimator.StopLootingAnim();
        }
        if (!_playerToolSelector.CanHarvest())
        {
            _playerAnimator.StopDigAnim();
        }
    }
    public void EnteredLoot(Crop crop)
    {
            _playerAnimator.PlayLootingAnim();
           // crop.Loot();
            // _crop.ScaleDown();
    }
    public void EnteredFish()
    {
        _playerAnimator.PlayFishingAnim();
    }
    public void EnteredRock(RockScript rock)
    {
        PlayerAnim.Instance.PlayDigngAnim();
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock")&&DigObject==null)
        {
            _isDigIn = true;
            DigObject = other.gameObject;
        }
        if (other.gameObject.CompareTag("Loot") && LootObject == null)
        {
            _isLooting = true;
            LootObject = other.gameObject;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Loot")&&LootObject!=null&&_isLooting&& _playerToolSelector.CanNone())
        {
            Crop crop = other.gameObject.GetComponent<Crop>();
            EnteredLoot(crop);
            _isLooting=false;
        }
        if (other.gameObject.CompareTag("Fish")&&!MobileJoystick.Instance.CanControl&& !FishingBoolean && _playerToolSelector.CanHarvest())
        {
            EnteredFish();
        }
        if(other.gameObject.CompareTag("Fish")  && (! _playerToolSelector.CanHarvest()|| MobileJoystick.Instance.CanControl))
        {
            _playerAnimator.StopFishingAnim();
        }

        if (other.gameObject.CompareTag("Rock") && DigObject != null && _isDigIn && _playerToolSelector.CanHarvest())
        {
            RockScript rock = other.gameObject.GetComponent<RockScript>();
            if(DigObject=rock.gameObject)
            {
                EnteredRock(rock);
                _isDigIn=false;
            }
        }
        if (other.gameObject.CompareTag("Rock") && DigObject == null)
        {
            _isDigIn = true;
            DigObject = other.gameObject;
        }
        if (other.gameObject.CompareTag("Rock") &&!_isDigIn&&!_playerToolSelector.CanHarvest())
        {
            _isDigIn = true;
            DigObject = other.gameObject;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Loot")&& LootObject!=null)
        {
            LootObject = null;
            _playerAnimator.StopLootingAnim();
            _isLooting = false;
        }
        if (other.gameObject.CompareTag("Rock") && DigObject != null)
        {
            DigObject = null;
            _playerAnimator.StopDigAnim();
            _isDigIn = false;
        }
        if (other.gameObject.CompareTag("Fish"))
        {
            _playerAnimator.StopFishingAnim();
            FishingBoolean = false;
        }
    }
}
