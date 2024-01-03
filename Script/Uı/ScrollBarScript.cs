using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarScript : MonoBehaviour
{
    private Scrollbar _scrollbar;
    public float initialValue = 0.5f;

    private void Awake()
    {
        _scrollbar = GetComponent<Scrollbar>();
    }
    void Start()
    {
        if (_scrollbar != null)
        {
            _scrollbar.value = initialValue;
        }
    }
}
