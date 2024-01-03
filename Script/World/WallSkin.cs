using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallSkin : MonoBehaviour
{
    public GameObject Wall;
    public void Walls()
    {
        if (!Wall.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
