using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim Instance;
    // Start is called before the first frame update
    [Header("Elements")]
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _waterParticles;

    [Header("Settings")]
    [SerializeField] private float _moveSpeedMultiplier;
    public bool MenuEnd;
    public CinemachineVirtualCamera VirtualCamera;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ManageAnimations(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            float speed = moveVector.magnitude * _moveSpeedMultiplier;
            if (speed > 1)
                speed = 1;
            _animator.SetFloat("moveSpeed",speed);
            PlayRunAnimation();
            // char ýn movedirection da hareket etmesi 
            _animator.transform.forward = moveVector.normalized;
        }
        else
        {
            PlayIdleAnimation();
        }
    }
    private void PlayRunAnimation()
    {
        _animator.Play("Run");
      //  Debug.Log("Run");
    }
    private void PlayIdleAnimation()
    {
        _animator.Play("Idle");
      //  Debug.Log("Idle");
    }
    public void PlaySowAnimation()
    {
        _animator.SetLayerWeight(1,1);
    }
    public void StopSowAnimation()
    {
        _animator.SetLayerWeight(1,0);
    }
    public void PlayWaterAnimation()
    {
        Debug.Log("Play Water Anim");
        _animator.SetLayerWeight(2,1);
    }
    public void StopWaterAnimation()
    {
        _animator.SetLayerWeight(2, 0);
        _waterParticles.Stop();
    }
    public void PlayHarvestAnimation()
    {
        _animator.SetBool("Harvest", true);
        _animator.SetLayerWeight(3, 1);
    }
    public void StopHarvestAnimation()
    {
        _animator.SetBool("Harvest", false);
        _animator.SetLayerWeight(3, 0);
    }
    public void PlayCuttingAnimation()
    {
        _animator.SetLayerWeight(4, 1);
    }
    public void StopCuttingAnimation()
    {
        _animator.SetLayerWeight(4, 0);
    }
    public void PlayLootingAnim()
    {
        _animator.Play("Loot", 5, 0f);
        _animator.SetLayerWeight(5, 1);
    }
    public void StopLootingAnim()
    {
        _animator.SetLayerWeight(5, 0);
    }
    public void PlayDigngAnim()
    {
        _animator.Play("Dig", 6, 0f);
        _animator.SetLayerWeight(6, 1);
        Debug.Log("Play dig");
    }
    public void StopDigAnim()
    {
        _animator.SetLayerWeight(6, 0);
    }
    public void PlayFishingAnim()
    {
        _animator.SetBool("Fishing", true);
        _animator.SetLayerWeight(7, 1);
    }
    public void StopFishingAnim()
    {
        _animator.SetBool("Fishing", false);
        _animator.SetLayerWeight(7, 0);
    }
    public void PlayMenuAnim()
    {
        _animator.SetLayerWeight(8, 0);
        PlayerHarvestAbility.Instance.gameObject.transform.eulerAngles = Vector3.zero;
        
       
    }
}
