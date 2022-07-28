using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLCanvas : MonoBehaviour
{
    public static DDOLCanvas instance;
    public GameObject pauseMenu;
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
