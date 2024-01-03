using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTree : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Sallama süresi
    public float shakeIntensity = 0.1f; // Sallama yoðunluðu

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
            // Shake süresi boyunca objeyi rastgele bir þekilde sallayýn
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = startPosition + randomOffset;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Shake süresi dolduðunda objeyi baþlangýç pozisyonuna geri getirin
            shakeTimer = 0f;
            transform.position = startPosition;
        }
    }

    // Shake iþlemini baþlatmak için bu metodu çaðýrabilirsiniz
    public void StartShake()
    {
        shakeTimer = shakeDuration;
    }
}
