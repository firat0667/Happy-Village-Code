using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;
    public AudioSource WaterSound;
    public AudioSource RockSound;
    public AudioSource TreeSound;
    public AudioSource BackGrounSound;
    public AudioSource MainMenuSound;
    public AudioSource CoinSound;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
     //  BackGrounSound.Play();
    }
}
