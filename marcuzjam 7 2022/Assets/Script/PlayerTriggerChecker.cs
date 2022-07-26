using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lava"))
        {
            PlayerManager.Instance.Kill();
        }
        else if (other.CompareTag("Arrow"))
        {
            PlayerManager.Instance.Kill();
        }
    }

}
