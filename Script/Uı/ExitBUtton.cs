using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitBUtton : MonoBehaviour
{
    private void Start()
    {
        // Butonun baðlý olduðu UI düðmesini buluyoruz
        Button exitButton = GetComponent<Button>();

        // Eðer buton varsa onClick event'ine oyunu kapatma fonksiyonunu baðlýyoruz
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("ExitButton scripti bir UI düðmesine eklenmelidir!");
        }
    }

    private void ExitGame()
    {
        // Oyunu kapat
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
