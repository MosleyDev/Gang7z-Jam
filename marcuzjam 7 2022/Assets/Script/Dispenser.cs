using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private float _arrowSpeed;

    private void SpawnArrow()
    {
        Vector2 spawnDirection = -transform.up;
        GameObject newArrow = Instantiate(_arrowPrefab, transform.position, Quaternion.AngleAxis(0, spawnDirection));

        newArrow.GetComponent<Rigidbody2D>().velocity = spawnDirection * _arrowSpeed;
    }
}
