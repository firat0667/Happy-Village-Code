using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SowSeason : MonoBehaviour
{
  public static SowSeason Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SeasonManager.Instance.SeasonChanged += SowSeasonChanged;
    }
    private void Start()
    {
        SowSeasonButton();
    }

    public void SowSeasonButton()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SeasonState seasonState = transform.GetChild(i).GetComponent<UiSowContainer>().SowData.SeasonState;
            if (SeasonManager.Instance.SeasonStateMain != seasonState)
                transform.GetChild(i).gameObject.SetActive(false);
            else
                transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        SeasonManager.Instance.SeasonChanged -= SowSeasonChanged;
    }
    public void SowSeasonChanged(SeasonState seasonStateMain)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SeasonState seasonState = transform.GetChild(i).GetComponent<UiSowContainer>().SowData.SeasonState;
            if (seasonStateMain != seasonState)
                transform.GetChild(i).gameObject.SetActive(false);
            else
                transform.GetChild(i).gameObject.SetActive(true);
        }
    }

   
}
