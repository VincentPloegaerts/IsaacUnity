using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    protected void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this as T;
    }
}
