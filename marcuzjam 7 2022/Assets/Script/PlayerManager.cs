using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Instance
    public static PlayerManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _deadBodyPrefab;

    public void Kill()
    {
        Vector2 deathPosisiton = transform.position;
        Instantiate(_deadBodyPrefab, deathPosisiton, Quaternion.identity);

        transform.position = _startPoint.position;
    }
}
