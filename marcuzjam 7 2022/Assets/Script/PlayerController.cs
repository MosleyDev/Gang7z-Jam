using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelAnimator;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private SpriteAnimation run;
    [SerializeField] private SpriteAnimation idle;
    [SerializeField] private SpriteAnimation runGrab_Up;
    [SerializeField] private SpriteAnimation runGrab_RightAndLeft;
    [SerializeField] private SpriteAnimation idleGrab_Up;
    [SerializeField] private SpriteAnimation idleGrab_RightAndLeft;
    [SerializeField] private SpriteAnimation jump;
    [SerializeField] private SpriteAnimation jump_To_Fall;
    [SerializeField] private AnimationManager anim;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform groundTransform;
    [Space(20)]
    [SerializeField] Rigidbody2D body;

    [SerializeField] private float speed, jumpForce;

    private float inputRaw;
    private bool isRun;

    public bool isGrabbingBody;
    public bool isGrabbingUp = true;
    private void Update()
    {
        inputRaw = Input.GetAxisRaw("Horizontal");
        bool groundCheck = Physics2D.OverlapBox(groundTransform.position, groundTransform.localScale, 0, groundLayer);

        isRun = Mathf.Abs(inputRaw) > 0 ? true : false;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isGrabbingUp = !isGrabbingUp;
        }

        if(groundCheck){

            if (Mathf.Abs(inputRaw) > 0)
            {

                anim.ChangeState(isGrabbingBody ? (isGrabbingUp ? runGrab_Up : runGrab_RightAndLeft) : run);

            }
            else
            {
                anim.ChangeState(isGrabbingBody ? (isGrabbingUp ? idleGrab_Up : idleGrab_RightAndLeft) : idle);
            }

        }
        if(groundCheck && !isGrabbingBody){

            if(Input.GetKeyDown(KeyCode.Space)){
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            }
        }

        if(!groundCheck){
            if(Mathf.Sign(body.velocity.y) >= 0)
                anim.ChangeState(jump);
            else if(Mathf.Sign(body.velocity.y) < 0)
                anim.ChangeState(jump_To_Fall);
        }

    
        if (inputRaw < 0)
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (inputRaw > 0)
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
    

    
    }


    private void FixedUpdate()
    {

        body.velocity = new Vector2(inputRaw * speed, body.velocity.y);


    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(groundTransform.position, groundTransform.localScale);
    }


}
