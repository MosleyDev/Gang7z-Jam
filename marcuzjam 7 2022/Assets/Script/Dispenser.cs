using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private DispenserDirection _dispenserDirection;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private float _arrowSpeed;

    private void SpawnArrow()
    {
        Vector2 spawnDirection = _dispenserDirection switch
        {
            DispenserDirection.up => transform.up,
            DispenserDirection.right => transform.right,
            DispenserDirection.down => -transform.up,
            DispenserDirection.left => -transform.right,
            _ => transform.up
        };

        GameObject newArrow = Instantiate(_arrowPrefab, transform.position, Quaternion.AngleAxis(0, spawnDirection));

        newArrow.GetComponent<Rigidbody2D>().velocity = spawnDirection * _arrowSpeed;
    }

    enum DispenserDirection
    {
        up, right, down, left
    }
}
