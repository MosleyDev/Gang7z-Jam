using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class SpriteAnimatorDataProvider{
    public static SpriteAnimatorDataProvider Instance {get; private set;} = new SpriteAnimatorDataProvider();
    private SpriteAnimatorDataProvider(){}

    
    public event EventHandler On_ChangedGroupVarible;
    public event EventHandler On_ChangedHitboxPropVarible;
    public event EventHandler On_AddedLayer;
    

    public bool isChangedGroupVarible;
    public bool isChangedHitboxPropVarible;
    public bool isAddedLayer;

    

    public void InvokeChangedGroup(){
        if(isChangedGroupVarible){
            On_ChangedGroupVarible?.Invoke(this, EventArgs.Empty);
            isChangedGroupVarible = false;  
        }

    }

    public void InvokeChangedHitboxProp(){
        if(isChangedHitboxPropVarible){
            On_ChangedHitboxPropVarible?.Invoke(this, EventArgs.Empty);
            isChangedHitboxPropVarible = false;
        }

    }

    public void InvokeAddedLayer(){
        if(isAddedLayer){
            On_AddedLayer?.Invoke(this, EventArgs.Empty);
            isAddedLayer = false;
        }
    }


    

}
