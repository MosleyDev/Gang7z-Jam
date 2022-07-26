using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelAnimator;

public class PlayerController : MonoBehaviour{

    [SerializeField] private SpriteAnimation run;
    [SerializeField] private SpriteAnimation idle;
    [SerializeField] private SpriteAnimation runGrab_Up;
    [SerializeField] private AnimationManager anim;
    
    [Space(20)]
    [SerializeField] Rigidbody2D body;

    [SerializeField] private float speed;

    private float inputRaw;
    private bool isRun;

    private void Start() {
        


    }

    private void Update() {
        inputRaw = Input.GetAxisRaw("Horizontal");

        isRun = Mathf.Abs(inputRaw) > 0 ? true : false; 

        if(Mathf.Abs(inputRaw) > 0){

            anim.ChangeState(run);

        }else{
            anim.ChangeState(idle);
        }

        if(inputRaw < 0)
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if(inputRaw > 0)
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    private void FixedUpdate() {
        
        body.velocity = new Vector2(inputRaw * speed, body.velocity.y);


    }


}
