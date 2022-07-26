using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelAnimator;

public class Lava : MonoBehaviour{ 

    [SerializeField] private AnimationManager anim; 
    [SerializeField] private SpriteAnimation lava;

    private void Start() {
        
    }
    private void Update() {
        anim.ChangeState(lava);

    }

}
