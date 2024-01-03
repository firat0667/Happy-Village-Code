using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeVision : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ChunkUnlockObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ChunkUnlockObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
