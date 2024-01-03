using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadActiveScene : MonoBehaviour
{
  public void YenidenYukle()
    {
        // Aktif sahnenin ad�n� al
        string aktifSahneAdi = SceneManager.GetActiveScene().name;

        // Aktif sahneyi yeniden y�kle
        SceneManager.LoadScene(aktifSahneAdi);
    }
}
