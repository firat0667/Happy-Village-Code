using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAnim : MonoBehaviour
{
    private VilalgerAI _villlagerAi;
    private GameObject _nearestTree;
    private void Start()
    {
       _villlagerAi = GetComponentInParent<VilalgerAI>();
        
    }
    public void CuttingTree()
    {
        _nearestTree = _villlagerAi.NearestTree;
        CropField go = _nearestTree.GetComponent<CropField>();
        if(go.TreeHarvestAmount != go.TreeHarvestIncrease)
        {
            if (go != null && go.CropType == CropType.Tree && go)
            {
                if (go.TreeHarvestAmount != go.TreeHarvestIncrease)
                    go.TreeHarvestIncrease++;
                if (go.CropParent != null && go.CropParent.childCount > 0)
                {
                  //  go.CropParent.GetChild(0).GetComponent<ShakeTree>().StartShake();
                    if (go.CropParent.GetChild(0).GetComponent<Crop>().CutParticlesTree)
                        go.CropParent.GetChild(0).GetComponent<Crop>().CutParticlesTree.Play();
                }
            }
        }
        else
        {
            go.GetComponentInChildren<CropTile>().TreeHarvestingAi();
            go.IsHarvested = true;
            go.TreeHarvestAmount = 0;
        }
    }
public void DigAnim()
    {
        _nearestTree = _villlagerAi.NearestTree;
        RockScript rock = _nearestTree.GetComponent<RockScript>();
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
    public void RepeatDigAnim()
    {
        _nearestTree = _villlagerAi.NearestTree;
        RockScript rock = _nearestTree.GetComponent<RockScript>();
        if (rock.DigAmount < rock.RockAmountMax)
        {
            _villlagerAi.StartDigAnim();
        }
    }
    public void Loot()
    {
        _villlagerAi.StopLootingAnim();
        _nearestTree = _villlagerAi.NearestTree;
        if ( _nearestTree != null )
        {
            Crop Mushroom = _nearestTree.GetComponent<Crop>();
                Mushroom.gameObject.SetActive(false);
                Mushroom.Loot();
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
}
