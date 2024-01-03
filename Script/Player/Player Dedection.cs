using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDedection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChunkTrigger"))
        {
            Chunk chunk = other.GetComponentInParent<Chunk>();
            chunk.TryUnlock();
        }
    }
}
