using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSFX : MonoBehaviour{

    [SerializeField] private AudioClip footSteps;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<Sprite> triggerFootStepsSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;




    private void Update() {
        if(triggerFootStepsSprites.Any(x => x == spriteRenderer.sprite) ){
            audioSource.PlayOneShot(footSteps);


        }else{

        }
    }


}
