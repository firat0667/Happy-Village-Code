using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropScaleSaver : MonoBehaviour
{
    // �l�e�i kaydetmek i�in anahtar
    private string _uniqueName;
    private string olcekAnahtar;
    public bool IsFruitTree;
    private static int _uniqueNameCounter = 0;
    private void Awake()
    {
        _uniqueName = "NesneOlcek" + _uniqueNameCounter.ToString();
        _uniqueNameCounter++;
        olcekAnahtar = GenerateUniqueKey(_uniqueName);
    }
    void Start()
    {
        // PlayerPrefs'ten kaydedilmi� bir �l�ek de�eri varsa kullan, yoksa varsay�lan� (0.2f) kullan
        float kaydedilmisOlcek = PlayerPrefs.GetFloat(olcekAnahtar, 0.2f);
        transform.localScale = new Vector3(kaydedilmisOlcek, kaydedilmisOlcek, kaydedilmisOlcek);
    }
    string GenerateUniqueKey(string uniqueName)
    {
        return "SavePlant_" + uniqueName;
    }
    void Update()
    {
        // Herhangi bir �l�ek de�i�ikli�i yapabilirsiniz
        float yeniOlcek = transform.localScale.x;
        transform.localScale = new Vector3(yeniOlcek, yeniOlcek, yeniOlcek);
    }

    void OnApplicationQuit()
    {
        // Uygulama kapat�ld���nda kaydet
        Kaydet();
    }

    // �l�e�i kaydet
   public void Kaydet()
    {
        float olcekDegeri = transform.localScale.x;
        PlayerPrefs.SetFloat(olcekAnahtar, olcekDegeri);
        if(gameObject.transform.localScale.x>=1f && !IsFruitTree)
            PlayerPrefs.DeleteKey(olcekAnahtar);
            PlayerPrefs.Save();
        
    }
}
