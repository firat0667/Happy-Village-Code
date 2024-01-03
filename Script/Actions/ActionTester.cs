using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTester : MonoBehaviour
{
    public static Action<int> MyAction;
    void Start()
    {
        MyAction?.Invoke(7);
    }
}
