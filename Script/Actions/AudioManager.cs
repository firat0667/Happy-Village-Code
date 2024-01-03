using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    void Awake()
    {
        ActionTester.MyAction += PlayTakeDamageSound;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlayTakeDamageSound(int health)
    {
        Debug.Log("Playing Taking Damage Sound");
    }
    private void OnDestroy()
    {
        ActionTester.MyAction -= PlayTakeDamageSound;
    }
}
