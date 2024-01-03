using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitBUtton : MonoBehaviour
{
    private void Start()
    {
        // Butonun ba�l� oldu�u UI d��mesini buluyoruz
        Button exitButton = GetComponent<Button>();

        // E�er buton varsa onClick event'ine oyunu kapatma fonksiyonunu ba�l�yoruz
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("ExitButton scripti bir UI d��mesine eklenmelidir!");
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
