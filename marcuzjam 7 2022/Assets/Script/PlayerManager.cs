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
    [SerializeField] private LayerMask _deadBodyLayer;
    [SerializeField] private Vector2 _dropOffset;
    [SerializeField] private float _grabRange;
    bool _isGrabbingBody;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isGrabbingBody)
            {
                DropBody();
            }
            else
            {
                Collider2D col = Physics2D.OverlapCircle(transform.position, _grabRange, _deadBodyLayer);
                if (col != null)
                {
                    if (col.GetComponent<DeadBody>().isOnLava == false)
                    {
                        GrabBody(col.gameObject);
                    }
                }
            }
        }
    }
    public void Kill()
    {
        Vector2 deathPosisiton = transform.position;
        Instantiate(_deadBodyPrefab, deathPosisiton, Quaternion.identity);

        transform.position = _startPoint.position;
    }

    public void GrabBody(GameObject body)
    {
        GetComponent<PlayerController>().isGrabbingBody = true;
        _isGrabbingBody = true;

        Destroy(body);
    }
    public void DropBody()
    {
        GetComponent<PlayerController>().isGrabbingBody = false;
        _isGrabbingBody = false;

        Vector3 spawnOffset = new Vector3(((transform.right.x > 0 ? 1 : -1) * _dropOffset.x), _dropOffset.y);
        Instantiate(_deadBodyPrefab, transform.position + spawnOffset, Quaternion.identity);
    }
}
