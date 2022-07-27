using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;


namespace PixelAnimator{

    public class AnimationManager : MonoBehaviour
    {

        private SpriteAnimation currentAnimation;
        private SpriteRenderer spriteRenderer;
        private SpriteAnimatorPreferences preferences;

        private float timer;
        private int activeFrame;
        private List<Layer> layers;
        private List<Sprite> sprites;
        [SerializeField]private bool loop;
        private float frameRate;
        private Action action;

        private List<GameObject> gameObjects;


        

        private void Start() {
            preferences = (SpriteAnimatorPreferences) EditorGUIUtility.Load("SpriteAnimatorPreferences.asset");
            spriteRenderer = GetComponent<SpriteRenderer>();
            sprites = new List<Sprite>();
            layers = new List<Layer>();
            gameObjects = new List<GameObject>();
            ApplyProperties();
            
        }

        private void Update() {


            ApplyProperties();
            Play();
            

        }

        private void Play(){
            timer += (Time.deltaTime * frameRate);
            if(timer >= 1f){
                timer -= 1f;
                activeFrame = (activeFrame + 1) % sprites.Count;
                if(!loop){
                    if(spriteRenderer.sprite == sprites[sprites.Count-1]){
                        Debug.Log("sadasd");
                        if(action != null)action();
                    }else{
                        spriteRenderer.sprite = sprites[activeFrame];
                    }
                        
                }else if(loop){
                    spriteRenderer.sprite = sprites[activeFrame];        

                }
            }
        }


        private void ApplyProperties(){

            if(layers.Count > 0){
                for(int i = 0; i < layers.Count; i++){
                    bool alreadyExist = false;
                    alreadyExist = gameObjects.Any(x => x.name == layers[i].group.boxType);
                    if(!alreadyExist){
                        gameObjects.Add(new GameObject(layers[i].group.boxType));
                        int index = gameObjects.Count -1;
                        gameObjects[index].AddComponent<BoxCollider2D>();
                        gameObjects[index].transform.parent = this.transform;
                        gameObjects[index].layer = layers[i].group.activeLayer;
                        gameObjects[index].GetComponent<BoxCollider2D>().sharedMaterial = layers[i].group.physicMaterial;
                        gameObjects[index].transform.localPosition = Vector3.zero;
                    }
                    
                }

                for(int i = 0; i < gameObjects.Count; i ++){
                    
                    
                    var animation = layers.Single(x => x.group.boxType == gameObjects[i].name);

                    var boxCol = gameObjects[i].GetComponent<BoxCollider2D>();
                    switch (animation.frames[activeFrame].colissionTypes){

                        case Frame.ColissionTypes.NoTrigger :
                            boxCol.isTrigger = false;
                        break;

                        default:
                            boxCol.isTrigger = true;
                        break;

                    }
                    
                    var size = animation.frames[activeFrame].hitboxRect.size;
                    var offset = animation.frames[activeFrame].hitboxRect.position;
                    var adjustedHitboxOffset = new Vector2(offset.x + size.x/2, size.y/2 + offset.y );

                    var adjustedXSize = (size.x * sprites[activeFrame].bounds.size.x)/sprites[activeFrame].rect.width;
                    var adjustedXOffset = ((adjustedHitboxOffset.x - sprites[activeFrame].rect.width/2)*sprites[activeFrame].bounds.size.x)/sprites[activeFrame].rect.width;
                    var adjustedYOffset = (((adjustedHitboxOffset.y - sprites[activeFrame].rect.height/2)*-1)*sprites[activeFrame].bounds.size.y)/sprites[activeFrame].rect.height;
                    boxCol.size = new Vector2(adjustedXSize, (size.y * sprites[activeFrame].bounds.size.y)/sprites[activeFrame].rect.height );

                    boxCol.offset = new Vector2( adjustedXOffset, adjustedYOffset);
                    
                    // var frame = layers[i].frames[activeFrame];
                    // for(int x = 0; x < currentAnimation.GetListOfProperties<Vector2>(layers[i].group.boxType, 277137203).Count; x++){
                    //     var data = currentAnimation.GetListOfProperties<Vector2>(layers[i].group.boxType, 277137203)[x].data;
                    //     var prop = preferences.hitboxProperties.FirstOrDefault(x => x.propID == 277137203);
                    
                    //     var component = (Rigidbody2D)GetComponent(prop.componentType.SystemType);
                    //     if(activeFrame == currentAnimation.GetListOfProperties<Vector2>(layers[i].group.boxType, 277137203)[x].frameIndex){

                    //         component.velocity = data;
                    //     }
                    // }
                

                }


            }
        }


        public void ChangeState(SpriteAnimation newState){
            if(currentAnimation != newState){
                currentAnimation = newState;
                activeFrame = 0;
                layers = newState.layers;
                loop = newState.loop;
                frameRate = newState.frameRate;
                sprites = newState.sprites;
            }
    
        }




    }
}
