
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    public Text fpsText;
    public float updateInterval = 0.5f; // FPS güncelleme aralýðý

    private float lastInterval;
    private int frames = 0;

    private void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    private void Update()
    {
        frames++;

        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            // FPS hesaplama
            float fps = frames / (timeNow - lastInterval);
            fpsText.text = "FPS: " + fps.ToString("F2"); // FPS'yi UI Text'e yazdýrýn
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
