using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLavaChecker : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _deadBodyPrefab;

    public void OnTriggerLava()
    {
        Vector2 deathPosisiton = transform.position;
        Instantiate(_deadBodyPrefab, deathPosisiton, Quaternion.identity);

        transform.position = _startPoint.position;
    }

    public static event Action onTriggerLava;
    private void OnEnable() => onTriggerLava += OnTriggerLava;
    private void OnDisable() => onTriggerLava -= OnTriggerLava;
    private void OnTriggerEnter2D(Collider2D other){ 


        
        if (other.CompareTag("Lava"))
        {
            onTriggerLava?.Invoke();
        }
    }

}
