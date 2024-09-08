using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyHandler : MonoBehaviour
{
    private static DontDestroyHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
