using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVillager : MonoBehaviour
{
    public float hareketHizi = 5f;
    public Transform[] hedefTransformListesi;
    public float durmaSuresi = 2f;

    public Animator animator; // D��ar�dan atanabilen Animator
    public Transform bakilacakTransform; // Karakterin durdu�u s�rada bak�lacak transform

    private float durmaBaslamaZamani;
    private int suankiHedefIndex = 0;

    void Start()
    {
        animator.SetLayerWeight(8, 1);
        YeniHedefBelirle();
    }

    void Update()
    {
        HareketEt();
    }

    void HareketEt()
    {
        if (hedefTransformListesi.Length == 0) return;

        Transform hedefTransform = hedefTransformListesi[suankiHedefIndex];
        float mesafe = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(hedefTransform.position.x, 0f, hedefTransform.position.z));

        if (mesafe > 0.1f)
        {
            // Y eksenini de�i�tirmeden hedefe do�ru hareket et
            Vector3 hareketYonu = (new Vector3(hedefTransform.position.x, 0f, hedefTransform.position.z) - new Vector3(transform.position.x, 0f, transform.position.z)).normalized;
            transform.Translate(hareketYonu * hareketHizi * Time.deltaTime, Space.World);
            transform.LookAt(new Vector3(hedefTransform.position.x, transform.position.y, hedefTransform.position.z));
        }
        else
        {
            // Hedefe ula��ld���nda durma s�resi ba�lat
            animator.SetBool("Kosma", false);

            if (Time.time > durmaBaslamaZamani)
            {
                // Belirli bir transforma bak
                Vector3 hedefYonu = (bakilacakTransform.position - transform.position).normalized;
                transform.LookAt(transform.position + hedefYonu, Vector3.up);

                if (Time.time < durmaBaslamaZamani + durmaSuresi)
                {
                    // Belirtilen s�re boyunca bekleyecek
                    // Burada ba�ka bir �ey yapabilirsiniz, �u anda sadece bekliyoruz
                }
                else
                {
                    // Belirtilen s�re sona erdi�inde bir sonraki hedefe ge�
                    suankiHedefIndex = (suankiHedefIndex + 1) % hedefTransformListesi.Length;
                    YeniHedefBelirle();
                }
            }
        }
    }

    void YeniHedefBelirle()
    {
        durmaBaslamaZamani = Time.time + durmaSuresi;
        animator.SetBool("Kosma", true);

        // Liste sonuna ula��ld�ysa ba�a d�n
        if (suankiHedefIndex == hedefTransformListesi.Length)
        {
            suankiHedefIndex = 0;
        }
    }
}
