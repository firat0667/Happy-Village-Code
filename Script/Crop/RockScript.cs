using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [Header("Elements")]
    public CropType CropType;
    public static Action<CropType, CropData> onCropHarvested;
    public ParticleSystem RockParticlesTree;
    public CropData CropData;
    public GameObject[] Rocks;
    private Collider _rockCollider;
    private BoxCollider _parentRock;
    public GameObject Rock;
    [Header("Settings")]
    public int DigAmount = 0;
    public int RockAmountMax = 3;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        RockChoice();
    }
    public void RockChoice()
    {
        // Rastgele bir kaya tipini seç
        int randomIndex = UnityEngine.Random.Range(0, Rocks.Length);

        for (int i = 0; i < Rocks.Length; i++)
        {
            GameObject kaya = Rocks[i];

            bool aktif = (i == randomIndex);
            kaya.transform.localPosition = new Vector3(0, 0, 0);
            kaya.SetActive(aktif);

            if (aktif)
            {
                _rockCollider = kaya.GetComponent<Collider>();
                Rock = kaya;
                Debug.Log(kaya.name + " aktif hale getirildi.");
            }
            else
            {
                Debug.Log(kaya.name + " pasif hale getirildi.");
            }
        }
    }
    public void DigIn()
    {
        EffectObjectPool.Instance.PlayParticle(transform.position, transform.rotation);
        onCropHarvested?.Invoke(CropData.CropType, CropData);
        DigAmount = 0;
        gameObject.SetActive(false);
        PlayerAnim.Instance.StopDigAnim();
        PlayerLootingAbility.Instance.DigObject = null;
        Rock=null;
    }
}
