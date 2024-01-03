using NaughtyAttributes;
using UnityEngine;

public class WheatColor : MonoBehaviour
{
    private Renderer[] childRenderers; // Child objelerin Renderers'larý
    private float _startScale;
    public Color startColor = Color.green;
    public Color endColor = Color.white;

    void Start()
    {
        _startScale = transform.localScale.x;

        // Parent obje deðilse child objelerin Renderers'larýný al
        GetChildRenderers();

        // Parent obje büyüme animasyonunu baþlat
        UpdateGrowth();
    }

    void GetChildRenderers()
    {
        // Parent objenin altýndaki tüm child objelerin Renderers'larýný al
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    void UpdateGrowth()
    {
        // Parent objenin boyutunu güncelle
        float currentScale = transform.parent.localScale.x;

        // Tüm child objelerin renk güncellemeleri
        foreach (Renderer childRenderer in childRenderers)
        {
            // Boyuta baðlý olarak renk deðiþimini yap
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
        // Parent objenin boyutunu güncelle
        float currentScale = transform.localScale.x;

        // Parent objenin boyutu deðiþtikçe renk deðiþimini güncelle
        UpdateGrowth(); 
    }
}
