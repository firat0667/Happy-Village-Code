using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ActionTester.MyAction += DisplayHealthPopup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DisplayHealthPopup(int health)
    {
        Debug.Log("Displaying health popup");
    }
    private void OnDestroy()
    {
        ActionTester.MyAction -= DisplayHealthPopup;
    }
}
