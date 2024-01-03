using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageParetToChild : MonoBehaviour
{
    private Image _myImage;
    public Image ParentImage;
    // Start is called before the first frame update
    private void OnEnable()
    {
        
    }
    void Start()
    {
        _myImage = GetComponent<Image>();
        _myImage.sprite=ParentImage.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
