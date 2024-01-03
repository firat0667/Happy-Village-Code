using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Elements")]
    public Transform CropRenderer;
    [SerializeField] private ParticleSystem _harvestedCornParticles;
    [SerializeField] private ParticleSystem _harvestedFruitTreeParticles;
    [SerializeField] private GameObject _CropRendererRawParticle;
    public ParticleSystem CutParticlesTree;
    public CropData CropData;
    public CropType CropType;
    public static Action<CropType, CropData> onCropHarvested;
    public bool IsFruit;
    public bool IsTree;
    private FruitSpawner _fruitSpawner;
    private Transform _parent;
   [HideInInspector]public bool IsBuild;
    [HideInInspector] public CropScaleSaver ScaleSaver;
    public bool Isharvested;
    public bool _firstrun;
    private CropField _cropfield;

    // Start is called before the first frame update

    private void OnEnable()
    {
        if (IsFruit && GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
        if (CropType == CropType.Tree)
        {
            GetComponentInParent<CropField>().ChildRenderer = CropRenderer;
            ScaleUp();
        }
       
        _cropfield = GetComponentInParent<CropField>();
        ScaleSaver = GetComponentInChildren<CropScaleSaver>();
    }
    void Start()
    {
       
        if (IsFruit)
        {
            _fruitSpawner = GetComponent<FruitSpawner>();
            if(transform.parent.transform.parent!=null)
            _parent = transform.parent.transform.parent;
          
        }
        if (IsFruit && CropRenderer.localScale.x < 1 && _parent == BuildManager.Instance.Builds)
        {
            LeanTween.scale(CropRenderer.gameObject, Vector3.one, CropData.GrownTime);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (IsFruit && CropRenderer.localScale.x >= 1 && SeasonManager.Instance.SeasonStateMain!=SeasonState.Winter)
            _fruitSpawner.enabled = true;  
        else
        {
            if(_fruitSpawner!=null)
            _fruitSpawner.enabled = false;
        }
        
        if(CropType != CropType.Flower && !_firstrun)
        {
            if (CropRenderer.localScale.x >= 1 && _CropRendererRawParticle != null)
            {
                _CropRendererRawParticle.gameObject.SetActive(true);
                _firstrun = true;
            }
           else
            {
                _CropRendererRawParticle.gameObject.SetActive(false);
                _firstrun = false;
            } 
        }
    }

    public void ScaleUpdate()
    {
        if (IsFruit && CropRenderer.localScale.x < 1) {
            LeanTween.scale(CropRenderer.gameObject, Vector3.one, CropData.GrownTime);
        }
    }

    public void Loot()
    {
        onCropHarvested?.Invoke(CropData.CropType, CropData);
        if (IsFruit)
        {
            gameObject.GetComponent<ObjectPool>().ReturnObject(gameObject);
        }
            

    }
    public void ScaleUp()
    {
        Isharvested = false;
        if (CropType != CropType.Tree)
        {
            float growntime = (1 / CropRenderer.localScale.x) * (CropData.GrownTime / 5);
            CropRenderer.gameObject.LeanScale(Vector3.one, growntime).setEase(LeanTweenType.clamp);
        }
        else
            CropRenderer.gameObject.LeanScale(Vector3.one, CropData.GrownTime).setEase(LeanTweenType.clamp);
           

    }
    public void StopGrowthAnimation()
    {
        // Animasyonu durdur
        LeanTween.cancel(CropRenderer.gameObject);
    }

    public void ScaleDown()
    {
        CropRenderer.gameObject.LeanScale(Vector3.zero, 0.9f)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(() => Destroy(gameObject));
        _harvestedCornParticles.gameObject.SetActive(true);
        _harvestedCornParticles.transform.parent=null;
        _harvestedCornParticles.Play();
        if(CropType!=CropType.Tree)
        ScaleSaver.Kaydet();
       
    }
    /*
    private void DestroyCrop() 
    {
      // yukarýda ()=> yerýne bu method da  yazýlabilir;
    }
    */
}
