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
        // Parent objenin alt�ndaki t�m child objeleri al
        Transform parentTransform = transform; // Parent objenin transform'u
        int childCount = parentTransform.childCount;

        // E�er en az bir child obje varsa devam et
        if (childCount > 0)
        {
            // Rastgele bir child objeyi se�
            int randomChildIndex = Random.Range(0, childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);

                // Se�ilen child objeyi aktif hale getir, di�erlerini deaktif hale getir
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
            Debug.LogError("Parent obje alt�nda hi� child obje yok!");
        }
    }
}
