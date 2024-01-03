using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCropInteractor : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Material[] materials;
    // Start is called before the first frame update
   
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetVector("_PlayerPosition",transform.position);
        }
    }
}
