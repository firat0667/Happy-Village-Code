using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroomchoice : MonoBehaviour
{
    private void OnEnable()
    {
        MushChoice();

    }
    public void MushChoice()
    {
        // Parent objenin altýndaki tüm child objeleri al
        Transform parentTransform = transform; // Parent objenin transform'u
        int childCount = parentTransform.childCount;

        // Eðer en az bir child obje varsa devam et
        if (childCount > 0)
        {
            // Rastgele bir child objeyi seç
            int randomChildIndex = Random.Range(0, childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);

                // Seçilen child objeyi aktif hale getir, diðerlerini deaktif hale getir
                if (i == randomChildIndex)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Parent obje altýnda hiç child obje yok!");
        }
    }
}
