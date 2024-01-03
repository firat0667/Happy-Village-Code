using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObjectSelected : MonoBehaviour
{
    public static BuildObjectSelected Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    

}
