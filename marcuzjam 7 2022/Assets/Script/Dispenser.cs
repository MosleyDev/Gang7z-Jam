using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private DispenserDirection _dispenserDirection;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private float _arrowSpeed, _spawnDuration;
    [SerializeField] private Transform _upSP, _rightSP, _downSP, _leftSP;

    private void Start()
    {
        StartCoroutine(ArrowLoop());
    }
    IEnumerator ArrowLoop()
    {
        while (true)
        {
            SpawnArrow();
            yield return new WaitForSeconds(_spawnDuration);
        }
    }
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

        Vector2 spawnPosition = _dispenserDirection switch
        {
            DispenserDirection.up => _upSP.position,
            DispenserDirection.right => _rightSP.position,
            DispenserDirection.down => _downSP.position,
            DispenserDirection.left => _leftSP.position,
            _ => _upSP.position
        };

        GameObject newArrow = Instantiate(_arrowPrefab, spawnPosition, Quaternion.AngleAxis(0, spawnDirection));

        newArrow.GetComponent<Rigidbody2D>().velocity = spawnDirection * _arrowSpeed;
    }

    enum DispenserDirection
    {
        up, right, down, left
    }
}
