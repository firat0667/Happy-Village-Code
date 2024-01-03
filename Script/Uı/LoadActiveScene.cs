using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadActiveScene : MonoBehaviour
{
  public void YenidenYukle()
    {
        // Aktif sahnenin adýný al
        string aktifSahneAdi = SceneManager.GetActiveScene().name;

        // Aktif sahneyi yeniden yükle
        SceneManager.LoadScene(aktifSahneAdi);
    }
}
