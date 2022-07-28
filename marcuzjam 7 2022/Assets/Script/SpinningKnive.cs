using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelAnimator;

public class SpinningKnive : MonoBehaviour
{
    [SerializeField] LayerMask _layer;
    [SerializeField] private float _speed, _rayDistance;
    [SerializeField] private AnimationManager anim; 
    [SerializeField] private SpriteAnimation spiningAnimation;
    Vector2 _currentDirection;
    Rigidbody2D _rigidbody2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _currentDirection = transform.right;
        _rigidbody2D.velocity = _currentDirection * _speed;
        
    
    }
    void SwitchDirection()
    {
        _currentDirection = -_currentDirection;
        _rigidbody2D.velocity = _currentDirection * _speed;
    }

    private void Update()
    {
        anim.ChangeState(spiningAnimation);    
        if (Physics2D.Raycast(transform.position, _currentDirection, _rayDistance, _layer))
        {
            SwitchDirection();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.Kill();
        }


    }
}
