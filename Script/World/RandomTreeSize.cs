using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreeSize : MonoBehaviour
{
    public float MinHeight = 0.35f; // Minimum aðaç yüksekliði
    public float MaxHeight = 0.85f; // Maksimum aðaç yüksekliði
    public float MinWidht = 0.15f;
    public float MaxWidht = 0.55f;

    void Start()
    {
        RandomizeTreeSizes();
    }

    void RandomizeTreeSizes()
    {
            float randomHeight = Random.Range(MinHeight, MaxHeight);
            float randomWidth = Random.Range(MinWidht, MaxWidht); // Rastgele geniþlik deðeri, isteðe baðlý

            gameObject.transform.localScale = new Vector3(randomWidth, randomHeight, randomWidth);
    }
}
