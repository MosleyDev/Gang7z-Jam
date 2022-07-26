using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePlate : MonoBehaviour{ 

    [SerializeField] private bool isPlayerTouched;
    [SerializeField] private SpriteRenderer render;
    private Sprite currenSprite;
    [SerializeField] private Sprite preasuredSprite;

    private void Start() {
        currenSprite = render.sprite;
    }

    private void Update() {
        
        if(isPlayerTouched){
            render.sprite = preasuredSprite;
        }else{
            render.sprite = currenSprite;
        }

    }


    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            isPlayerTouched = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            isPlayerTouched = false;
        }
    }


}
