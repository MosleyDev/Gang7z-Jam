using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLEventSystem : MonoBehaviour
{
    private static DDOLEventSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
