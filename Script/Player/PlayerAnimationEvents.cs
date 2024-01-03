using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ParticleSystem _seedParticle;
    [SerializeField] private ParticleSystem _waterParticle;
    // Start is called before the first frame update
    [Header("Events")]
    [SerializeField] private UnityEvent _startHarvestingEvent;
    [SerializeField] private UnityEvent _stopHarvestingEvent;

    public void PlaySeedParticles()
    {
        _seedParticle.Play();
    }
    public void PlayWaterParticles()
    {
        _waterParticle.Play();
        SoundManager.Instance.WaterSound.Play();
    }
    public void StartHarvestingCallBack()
    {
        _startHarvestingEvent?.Invoke();
        CropField go = PlayerHarvestAbility.Instance.CurrentCropField;
        if (go != null && go.CropType == CropType.Tree && go)
        {
            go.TreeHarvestIncrease++;
            SoundManager.Instance.TreeSound.Play();
            if (go.CropParent != null && go.CropParent.childCount > 0)
            {
                go.CropParent.GetChild(0).GetComponent<ShakeTree>().StartShake();
                if (go.CropParent.GetChild(0).GetComponent<Crop>().CutParticlesTree)
                    go.CropParent.GetChild(0).GetComponent<Crop>().CutParticlesTree.Play();
            }
        }

    }
    public void TreeSound()
    {
        SoundManager.Instance.TreeSound.Play();
    }
    public void RockSound()
    {
        SoundManager.Instance.RockSound.Play();
    }

    public void StopHarvestingCallBack()
    {
        _stopHarvestingEvent?.Invoke();
    }
    public void StoplootAnim()
    {
        PlayerAnim.Instance.StopLootingAnim();
        if (PlayerLootingAbility.Instance.LootObject != null)
        {

            PlayerLootingAbility.Instance.LootObject.SetActive(false);
            PlayerLootingAbility.Instance.LootObject.GetComponent<Crop>().Loot();
            PlayerLootingAbility.Instance.LootObject = null;
        }
    }
    public void StartDigAnim()
    {
       if (PlayerLootingAbility.Instance.DigObject != null)
        {
          RockScript rock =PlayerLootingAbility.Instance.DigObject.GetComponent<RockScript>();
            if (rock.DigAmount < rock.RockAmountMax)
            {
                PlayerAnim.Instance.PlayDigngAnim();
            }

        }
    }
    public void DigAnimOnRock()
    {

            RockScript rock = PlayerLootingAbility.Instance.DigObject.GetComponent<RockScript>();
            if (rock.DigAmount < rock.RockAmountMax)
            {
                rock.DigAmount++;
                rock.Rock.GetComponent<ShakeTree>().StartShake();
               
            if (rock.RockParticlesTree != null) // Eriþimden önce kontrol ekleyin
            {
               rock.RockParticlesTree.Play();
            }
            
            }
            if (rock.DigAmount >= rock.RockAmountMax)
                rock.DigIn();
    }
    public void FishingAnim()
    {
        PlayerStat.Instance.LoadPlayerData();
        // cropdata yý yap crop u yap fis direk ekle crop type dan 
        int random = Random.Range(1, 11);
        int fishingAbilityValue = PlayerStat.Instance.FishingAbility*2;
        Debug.Log(random + " " + fishingAbilityValue);
        if (fishingAbilityValue >= random)
           StartCoroutine(CatchFish());
        
        else
            StartCoroutine(DontCatchFish());
        
            
        
        
    }
    public void RepeatFishingAnim()
    {
        PlayerLootingAbility.Instance.FishingBoolean = false;
    }
    public void StartFishingAnim()
    {
        PlayerLootingAbility.Instance.FishingBoolean = true;
    }

    IEnumerator CatchFish()
    {
        PlayerLootingAbility.Instance.FishLooting();
        PlayerLootingAbility.Instance.FishParent.SetActive(true);
        Image image = PlayerLootingAbility.Instance.FishImageLane.GetComponent<Image>();
        image.sprite = PlayerLootingAbility.Instance.FishCorrectImage;
        image.enabled = true;
        PlayerLootingAbility.Instance.FishParticle.transform.position = PlayerLootingAbility.Instance.FishingSphere.transform.position;
        PlayerLootingAbility.Instance.FishParticle.gameObject.transform.LookAt(PlayerLootingAbility.Instance.transform);
        PlayerLootingAbility.Instance.FishParticle.gameObject.SetActive(true);
        PlayerLootingAbility.Instance.FishParticle.Play();
        yield return new WaitForSeconds(1f);
        PlayerLootingAbility.Instance.FishParticle.Stop();
        image.enabled = false;

    }
    IEnumerator DontCatchFish()
    {
        Image image = PlayerLootingAbility.Instance.FishImageLane.GetComponent<Image>();
        image.sprite = PlayerLootingAbility.Instance.FishErrorImage;
        image.enabled = true;
        PlayerLootingAbility.Instance.FishParent.SetActive(false);
        yield return new WaitForSeconds(1f);
        image.enabled = false;
    }
   
}
