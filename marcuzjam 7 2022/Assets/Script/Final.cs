using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelAnimator;


public class Final : MonoBehaviour{

    [SerializeField] private SpriteAnimation death;
    [SerializeField] private AnimationManager anim;



    private void Update() {
        anim.ChangeState(death);
    }


}
