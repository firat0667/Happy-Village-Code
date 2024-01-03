using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockChildObject : MonoBehaviour
{
    // Start is called before the first frame update,
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
