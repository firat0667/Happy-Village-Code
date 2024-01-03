using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTree : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Sallama s�resi
    public float shakeIntensity = 0.1f; // Sallama yo�unlu�u

    private Vector3 startPosition;
    private float shakeTimer = 0f;
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
    }
    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // Shake s�resi boyunca objeyi rastgele bir �ekilde sallay�n
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = startPosition + randomOffset;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Shake s�resi doldu�unda objeyi ba�lang�� pozisyonuna geri getirin
            shakeTimer = 0f;
            transform.position = startPosition;
        }
    }

    // Shake i�lemini ba�latmak i�in bu metodu �a��rabilirsiniz
    public void StartShake()
    {
        shakeTimer = shakeDuration;
    }
}
