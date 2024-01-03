using NaughtyAttributes;
using UnityEngine;

public class WheatColor : MonoBehaviour
{
    private Renderer[] childRenderers; // Child objelerin Renderers'lar�
    private float _startScale;
    public Color startColor = Color.green;
    public Color endColor = Color.white;

    void Start()
    {
        _startScale = transform.localScale.x;

        // Parent obje de�ilse child objelerin Renderers'lar�n� al
        GetChildRenderers();

        // Parent obje b�y�me animasyonunu ba�lat
        UpdateGrowth();
    }

    void GetChildRenderers()
    {
        // Parent objenin alt�ndaki t�m child objelerin Renderers'lar�n� al
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void UpdateGrowth()
    {
        // Parent objenin boyutunu g�ncelle
        float currentScale = transform.parent.localScale.x;

        // T�m child objelerin renk g�ncellemeleri
        foreach (Renderer childRenderer in childRenderers)
        {
            // Boyuta ba�l� olarak renk de�i�imini yap
            float t = Mathf.Clamp01((currentScale - 0.2f) / (1.0f - 0.2f));
            Color lerpedColor = Color.Lerp(startColor, endColor, t);
            childRenderer.material.SetColor("_Base_Map_Color", lerpedColor);
            childRenderer.material.SetVector("_PlayerPosition", PlayerSowAbility.Instance.gameObject.transform.position);
            childRenderer.material.SetFloat("_Effect_Radius", 0.3f);
            childRenderer.material.SetFloat("_Effect_Amplitude", 0.35f);
        }
    }

    void Update()
    {
        // Parent objenin boyutunu g�ncelle
        float currentScale = transform.localScale.x;

        // Parent objenin boyutu de�i�tik�e renk de�i�imini g�ncelle
        UpdateGrowth(); 
    }
}
