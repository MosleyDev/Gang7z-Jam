 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Net.Mime;
using System.Reflection;



namespace PixelAnimator{ 

    [Serializable]
    public class SpriteAnimatorWindow : EditorWindow{

        
        #region Var
        private Rect timelineRect, dragTimelineRect, topTimelineRect, addGroupsRect, backRect, playRect, frontRect, yLayoutLine, xLayoutLine;

        private Rect[] yLineRect,  frameRect;
        private Rect[,] selectFrameRect;

        private List<GroupRects> groupRects;
        private Sprite activeSprite;
        private SpriteAnimation selectedAnimation{get {return SpriteAnimatorUtility.spriteAnim;} set{SpriteAnimatorUtility.spriteAnim = value;}} 

        private float zoomScale = 1, zoomFactor = 1;
        private float timer, editorDeltaTime;
        private int activeFrame, selectedGroup;
        private float lastTimeSinceStartup = 0f;
        private Color blackColor;
        private bool isPlaying, checkSameProp, draggableTimeline;
        private Vector2 pivotPoint, timeLinePosition;
        private SpriteAnimatorPreferences sp;
        
        private Texture2D backT, playT, frontT, defAddGroupsT, onMAddGroupsT, dropdownT, selectedFrameT, singleFrameT;

        private List<Texture2D> dropdwn;
        private GenericMenu settingsPopup;
        private SerializedObject  spriteAnim;
        private SerializedProperty propGroups;

        private int frameCount, groupCount;
        private bool startCreate, deleteGroup, bringToUpGroup, bringToDownGroup, changeGroupType;
        private float thumbnailScaLe;
        private int test;
        private Group changedType;
        private float scale = 1;

        private Vector2 greatestSize;
        private int selectedFrame;


    

        #endregion
        

        [MenuItem("Window/SpriteAnimator")]
        private static void Init(){
            SpriteAnimatorWindow spriteAnimatorWindow =  GetWindow<SpriteAnimatorWindow>();
        }
    
        
        private void OnEnable(){

            //set texture
            // sp = (SpriteAnimatorPreferences) EditorGUIUtility.Load("SpriteAnimatorPreferences.asset");
            sp = (SpriteAnimatorPreferences)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/SpriteAnimatorPreferences.asset", typeof(SpriteAnimatorPreferences));
            backT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Back.png", typeof(Texture2D)) ;
            frontT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Front.png", typeof(Texture2D));
            defAddGroupsT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/AddBoxes.png", typeof(Texture2D));
            onMAddGroupsT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/AddBoxes2.png", typeof(Texture2D));
            dropdownT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/drop.png", typeof(Texture2D));
            selectedFrameT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/selectedFrame.png", typeof(Texture2D));
            singleFrameT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/frame.png", typeof(Texture2D));
            
            float buttonSize = 32;// button rect set
            addGroupsRect = new Rect(15, 15, 48, 48);
            backRect = new Rect(200, 20, buttonSize, buttonSize);
            playRect = new Rect((backRect.width + backRect.xMin) + 2, backRect.yMin, buttonSize, buttonSize); 
            frontRect = new Rect((playRect.width + playRect.xMin) + 2, backRect.yMin, buttonSize, buttonSize);

            blackColor = new Color(0.15f, 0.15f, 0.15f, 1);
            settingsPopup = new GenericMenu();
            
            groupRects = new List<GroupRects>();
            dropdwn = new List<Texture2D>();
            minSize = new Vector2( 700, 400 );

            
            startCreate = true;
            // set event
            SpriteAnimatorDataProvider.Instance.On_ChangedGroupVarible += SetAddGroupPopup;
            SpriteAnimatorDataProvider.Instance.On_ChangedGroupVarible += SyncGroups;
            // SpriteAnimatorDataProvider.Instance.On_AddedLayer += SetAddGroupPopup;

            // set group menu
            settingsPopup.AddItem(new GUIContent("Settings/Delete"), false, () => deleteGroup = true);
            settingsPopup.AddItem(new GUIContent("Settings/Bring to up"), false, () => bringToUpGroup = true);
            settingsPopup.AddItem(new GUIContent("Settings/Bring to down"), false, () => bringToDownGroup = true);
            wantsMouseMove = true;

        }
        float determinant;

        private void OnInspectorUpdate() => Repaint();

        private void OnGUI() {

            var e = Event.current;



            foreach(UnityEngine.Object obj in Selection.objects){

                if(obj is SpriteAnimation){

                    if(selectedAnimation != (SpriteAnimation) obj){
                        var tempAnimation = (SpriteAnimation) obj;

                        spriteAnim = new SerializedObject(tempAnimation);
                        activeSprite = tempAnimation.sprites[0];
                        timer = 0;
                        checkSameProp = false;
                        frameCount = tempAnimation.sprites.Count;
                        yLineRect = new Rect[tempAnimation.sprites.Count + 1];

                        SpriteAnimatorDataProvider.Instance.isChangedGroupVarible = true;
                        SpriteAnimatorDataProvider.Instance.isChangedHitboxPropVarible = true;
                        selectedAnimation = tempAnimation;
                    }
                    
                }

            }
            // Key Event
            if (e.isKey){
                if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Return){ 
                    isPlaying = !isPlaying;
                    
                    Repaint();
                } 
            }
            else if((e.keyCode == KeyCode.LeftArrow && e.type == EventType.KeyDown)){
                if(activeFrame != 0){
                    activeFrame --;
                    activeSprite = selectedAnimation.sprites[activeFrame];
                }
                else if(activeFrame == 0){
                    int index = selectedAnimation.sprites.Count - 1; 
                    activeSprite = selectedAnimation.sprites[ index ];
                    activeFrame = index;
                }
            }
            else if((e.keyCode == KeyCode.RightArrow && e.type == EventType.KeyDown)){
                activeFrame = (activeFrame + 1) % selectedAnimation.sprites.Count;
                activeSprite = selectedAnimation.sprites[activeFrame];
            }
            if(selectedAnimation.layers.Count > 0)SetLayerMenu();
            SpriteAnimatorDataProvider.Instance.InvokeChangedGroup();

            ApplyAnimationChanges();
            SetWindows();

        }

        private void ApplyAnimationChanges(){
                    // Checking the frame count
            var sprites = selectedAnimation.sprites;
            var layers = selectedAnimation.layers;
            frameRect = new Rect[sprites.Count * layers.Count];
            if(frameCount != sprites.Count){

                yLineRect = new Rect[sprites.Count + 1];
                frameRect = new Rect[sprites.Count * layers.Count];
                frameCount = sprites.Count;
            }
            // Checking the group count
            if(groupCount != layers.Count){


                frameRect = new Rect[sprites.Count * layers.Count];
                propGroups = spriteAnim.FindProperty("groups");

                groupCount = layers.Count;

            }

            
            // Start Create
            if(frameCount == sprites.Count && groupCount == layers.Count && startCreate){


                yLineRect = new Rect[sprites.Count + 1];            
                propGroups = spriteAnim.FindProperty("groups");
                startCreate = false;
                
            }

                
        }

        private void SetWindows(){
            var e = Event.current;
            pivotPoint = new Vector2(position.width / 2, -3 );

            BeginWindows();
            
            var tempColor = GUI.color;
            GUI.color = new Color(0, 0, 0, 0.2f);
            GUI.Window(4, new Rect(10, 10, 360, 280), DrawHitboxProperties, "");
            GUI.color = tempColor;
            if(Event.current.type == EventType.ScrollWheel){
                float scale = Mathf.Sign(Event.current.delta.y) == 1 ? -1 : 1;
                zoomScale += scale;
                zoomScale = Mathf.Clamp(zoomScale, 0f, 4f);
                Repaint();

            }

            zoomFactor = timelineRect.position.y/200;
            
            CreateTimeline();
            // EditorGUI.LabelField(new Rect(700, 80, 120, 120), zoomScale.ToString());

            EndWindows();
            var tempMatrix = GUI.matrix;
            GUIUtility.ScaleAroundPivot( Vector2.one * zoomScale * zoomFactor,  pivotPoint);  
            GUI.Window(1, new Rect( position.width/2 - activeSprite.rect.width/2 , 0 , activeSprite.rect.width, activeSprite.rect.height), CreateSpriteTexture, "");

            GUI.matrix = tempMatrix;

            Repaint();


            
            GUI.BringWindowToFront(4);
            GUI.BringWindowToFront(2);
            GUI.BringWindowToBack(1);

        }

        
        private void CreateSpriteTexture(int windowID){
            var e = Event.current;
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, activeSprite.rect.width, activeSprite.rect.height), AssetPreview.GetAssetPreview(activeSprite), ScaleMode.ScaleToFit);
            AssetPreview.GetAssetPreview(activeSprite).filterMode = FilterMode.Point;
            
        
            spriteAnim.Update();
            if(selectedAnimation.layers.Count > 0){


                for(int i = 0; i < selectedAnimation.layers.Count; i++){
                    var layer = selectedAnimation.layers[i];
                    var frame = layer.frames[activeFrame];
                    if(frame.hitboxRect.Contains(e.mousePosition) && e.button == 0 && e.type == EventType.MouseDown){
                        selectedGroup = i;
                    }
                    else if(selectedGroup != i){
                        layer.reSizeBox = false;
                    }
                    
                    frame.hitboxRect.width  = Mathf.Clamp( frame.hitboxRect.width, 0, float.MaxValue );
                    frame.hitboxRect.height = Mathf.Clamp( frame.hitboxRect.height, 0, float.MaxValue);

                    SpriteAnimatorUtility.DrawBox(layer, layer.group.color, activeFrame, layer.group.rounded);
                }
            }
            selectedAnimation.layers[selectedGroup].reSizeBox = true;
            spriteAnim.ApplyModifiedProperties();
    
        }

        private void SyncGroups(object sender, EventArgs e){
            var layers = selectedAnimation.layers;
            for(int x = 0; x < sp.groups.Count; x++){  
                for(int i = 0; i < layers.Count; i ++){
                    var group = layers[i].group;
                    if(group.groupID == sp.groups[x].groupID){
                        group = sp.groups[x];
                    }

                }

            }

        }

        #region Hitbox
            
        private Vector2 propScrollPos = Vector2.one; 
        private void DrawHitboxProperties(int windowID){
            
            if(selectedAnimation.layers.Count > 0)    {       
                using(var scroll = new EditorGUILayout.ScrollViewScope(propScrollPos)){
                    EditorGUI.LabelField(new Rect(10, 6, 120, 20), "Hitbox Properties", EditorStyles.boldLabel);
                    EditorGUI.DrawRect( new Rect(7, 30, 300, 2f), new Color(0.3f, 0.3f, 0.3f, 0.6f) );
                    GUILayout.Space(30);
                    propScrollPos = scroll.scrollPosition;

                    using(new GUILayout.HorizontalScope()){        
                        GUILayout.Space(20);
                        using(new GUILayout.VerticalScope()){
                            
                            var hitboxProp = spriteAnim.FindProperty("layers").GetArrayElementAtIndex(selectedGroup).FindPropertyRelative("frames").GetArrayElementAtIndex(activeFrame);
                            GUILayout.Space(20);
                            EditorGUILayout.PropertyField(hitboxProp.FindPropertyRelative("colissionTypes"));
                            
                            spriteAnim.ApplyModifiedProperties();
                            using(new GUILayout.HorizontalScope()){
                                spriteAnim.Update();
                                EditorGUILayout.LabelField("Box", GUILayout.Width(70));
                                EditorGUILayout.PropertyField(hitboxProp.FindPropertyRelative("hitboxRect"), GUIContent.none, GUILayout.Width(140), GUILayout.MaxHeight(60));
                                spriteAnim.ApplyModifiedProperties();
                            }


                            var hitboxData = selectedAnimation.layers[selectedGroup].frames[activeFrame].hitboxData;

                            for(int i = 0; i < sp.hitboxProperties.Count; i++){
                                var prop = sp.hitboxProperties[i];
                                var dataType = prop.dataType.SystemType;
                        
                            
                                using(new GUILayout.HorizontalScope()){
                                    
                                    EditorGUILayout.LabelField(prop.name, GUILayout.MaxWidth(120));
                                    
                                    SpriteAnimatorUtility.DrawProperty(hitboxProp.FindPropertyRelative("hitboxData"),dataType, sp.hitboxProperties[i].propID, hitboxData);

                                    GUILayout.Space(10);
                                    if(GUILayout.Button("X", GUILayout.MaxWidth(15), GUILayout.MaxHeight(15))){
                                        SpriteAnimatorUtility.AddProperty(hitboxData, prop);
                                        
                                    }
                                        
                                }
                            }

                            spriteAnim.ApplyModifiedProperties();
                        }
                    }
                }
            }

        }




        #endregion



        #region Timeline
        private void CreateTimeline(){

            var e = Event.current;
            topTimelineRect = new Rect( timelineRect.xMin, timelineRect.y - 10, timelineRect.xMax - timelineRect.xMin, 10 );
            dragTimelineRect = new Rect(0, topTimelineRect.y + 5, topTimelineRect.width, 10);

            timelineRect.size = new Vector2(position.width + 10, position.height + 100 );
            float clampYPosition = Mathf.Clamp(timelineRect.position.y, 200, position.height - 200);
            timelineRect.position = new Vector2(0, clampYPosition);


            EditorGUIUtility.AddCursorRect( dragTimelineRect, MouseCursor.ResizeVertical );
            
            
            if(e.type == EventType.MouseDrag){
                if(dragTimelineRect.Contains(e.mousePosition)) draggableTimeline = true;
                Repaint();
            }else if( e.type == EventType.MouseUp ){
                draggableTimeline = false;
            }
            if(draggableTimeline && e.type == EventType.MouseDrag){
                if(e.mousePosition.y < timelineRect.position.y && Math.Sign(e.delta.y) == -1 ) 
                    timeLinePosition = new Vector2(timelineRect.x, clampYPosition + e.delta.y);
                if(e.mousePosition.y > timelineRect.position.y && Mathf.Sign(e.delta.y) == 1)
                    timeLinePosition = new Vector2(timelineRect.x, clampYPosition + e.delta.y);
                timelineRect.position = timeLinePosition;

            }

            timelineRect = GUILayout.Window( 2, timelineRect, TimelineWindow, "");


        }

        private void TimelineWindow(int windowID){
            var e = Event.current;

            yLayoutLine = new Rect(frontRect.xMax + 15, 10, 5, timelineRect.height);
            
            EditorGUI.DrawRect(new Rect(0,0, timelineRect.width, timelineRect.height), new Color(0.1f,0.1f,0.1f,1));
            EditorGUI.DrawRect(new Rect(0,0, timelineRect.width, 10), blackColor);

            // Spritelarin icinde ki en buyuk size degerine sahip olan degeri aliyor

            float x = selectedAnimation.sprites.Aggregate((current, next) => current.rect.size.x > next.rect.size.x ? current : next).rect.size.x;
            float y = selectedAnimation.sprites.Aggregate((current, next) => current.rect.size.y > next.rect.size.y ? current : next).rect.size.y;
            greatestSize = new Vector2(x*scale, y* scale);

            // Thumbnail Scale 
            using(var change = new EditorGUI.ChangeCheckScope()){

                scale = EditorGUI.FloatField(new Rect(10, 250, 120, 30),scale);
                if(change.changed){
                    greatestSize *= scale;
                }

            }

            // 1.satiri ayirma cizgisi
            xLayoutLine = new Rect(0, greatestSize.y + 10, timelineRect.width, 8);

            EditorGUI.DrawRect(xLayoutLine, blackColor );
            EditorGUI.DrawRect(yLayoutLine, blackColor);

            if(!isPlaying)playT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Play.png", typeof(Texture2D)) ;
            else playT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Playing.png", typeof(Texture2D));

            // 1.Satir butonlari
            using(new GUILayout.HorizontalScope()){
                if(SpriteAnimatorUtility.Button(defAddGroupsT, onMAddGroupsT, addGroupsRect )){
                    SpriteAnimatorUtility.boxTypePopup.ShowAsContext();
                }
                else if(SpriteAnimatorUtility.Button(backT, backRect)){
                    if(activeFrame != 0){
                        activeFrame --;
                        activeSprite = selectedAnimation.sprites[activeFrame];
                    }
                    else if(activeFrame == 0){
                        int index = selectedAnimation.sprites.Count - 1; 
                        activeFrame = index;
                        activeSprite = selectedAnimation.sprites[ activeFrame ];
                    }
                }
                else if(SpriteAnimatorUtility.Button(playT, playRect)){
                    isPlaying = !isPlaying;
                    playT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Playing.png", typeof(Texture2D));
                }
                else if(SpriteAnimatorUtility.Button(frontT, frontRect)){
                    activeFrame = (activeFrame + 1) % selectedAnimation.sprites.Count;
                    activeSprite = selectedAnimation.sprites[activeFrame];
                }

            }

            DrawLayer();
            SetFrameThumbnail(greatestSize);
            if(selectedAnimation.layers.Count > 0) SetFrames();

        }


        private void SetAddGroupPopup(object sender, EventArgs e){

            SpriteAnimatorUtility.boxTypePopup = new GenericMenu();
            settingsPopup = new GenericMenu();

            settingsPopup.AddItem(new GUIContent("Settings/Delete"), false, () => deleteGroup = true);
            settingsPopup.AddItem(new GUIContent("Settings/Bring to up"), false, () => bringToUpGroup = true);
            settingsPopup.AddItem(new GUIContent("Settings/Bring to down"), false, () => bringToDownGroup = true);
            
            for(int i = 0; i < sp.groups.Count; i ++){

                SpriteAnimatorUtility.boxTypePopup.AddItem(new GUIContent(sp.groups[i].boxType), false, (object userData) => {
                        
                        var addedGroup = sp.groups.Single(g => g.boxType == userData.ToString());

                        if(selectedAnimation.layers == null) selectedAnimation.layers = new List<Layer>();
                        selectedAnimation.AddLayer(addedGroup);

                        dropdwn.Add( (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/drop.png", typeof(Texture2D)));
                        SpriteAnimatorDataProvider.Instance.isChangedGroupVarible = true;
                    
                    }
                , sp.groups[i].boxType);
            

                settingsPopup.AddItem(new GUIContent("Change Type/" + sp.groups[i].boxType), false, (object userData) =>{
                    changedType = userData as Group;
                    changeGroupType = true;

                }, sp.groups[i] );

            }


        }


        private void SetFrameThumbnail(Vector2 greatestSize){
            var e = Event.current;

            // First y axis line rect
            yLineRect[0] = new Rect(yLayoutLine.xMax + greatestSize.x, 10, 5, greatestSize.y);
            for(int i = 0; i < selectedAnimation.sprites.Count; i++){
                if(i>0)yLineRect[i] = new Rect(yLineRect[i-1].xMax + greatestSize.x, 10, 5, greatestSize.y);


                if(i != selectedAnimation.sprites.Count){
                    var sprite = selectedAnimation.sprites[i]; 
                    
                    Rect spriteRect;
                    Rect transparentRect;
                    Rect spriteSelectableRect;
                    float width = sprite.rect.width;
                    float height = sprite.rect.height;
                    float adjustedSpriteWidth  = sprite.rect.width * scale;
                    float adjustedSpriteHeight = sprite.rect.height * scale ;
                    float adjustedSpriteXPos   = (yLineRect[i].xMin - greatestSize.x) + (greatestSize.x/2 - adjustedSpriteWidth/2) ;
                    float adjustedSpriteYPos   = ( greatestSize.y/2 - adjustedSpriteHeight/2 ) + 10;

                    

                    // Set sprite x pos
                    adjustedSpriteXPos = (yLineRect[i].xMin - greatestSize.x) + (greatestSize.x/2 - adjustedSpriteWidth/2); 

                    // Set sprite y pos
                    adjustedSpriteYPos = ( greatestSize.y/2 - adjustedSpriteHeight/2 ) + 10;

                        
                    spriteRect = new Rect(adjustedSpriteXPos, adjustedSpriteYPos, adjustedSpriteWidth, adjustedSpriteHeight);
                    transparentRect = new Rect( yLineRect[i].xMin - greatestSize.x , yLineRect[i].yMin, greatestSize.x, timelineRect.height);
                    spriteSelectableRect = new Rect(adjustedSpriteXPos, adjustedSpriteYPos, greatestSize.x, greatestSize.y);


                
                    GUI.DrawTexture(spriteRect, AssetPreview.GetAssetPreview(sprite));
                    
                    if(activeFrame == i){
                        EditorGUI.DrawRect(transparentRect, new Color(255, 255, 255, 0.2f));
                        
                    }
                    if(spriteSelectableRect.Contains(e.mousePosition) && e.button == 0 && e.type == EventType.MouseDown && activeFrame != i){
                        activeSprite = sprite;
                        activeFrame = i;
                    } 

                        
                }
                EditorGUI.DrawRect(yLineRect[i], blackColor);
            }
        }
        

        private void DrawLayer(){

            var e = Event.current;
            var width = yLayoutLine.xMin - xLayoutLine.xMin;
            var height = 45;
            var lineHeight = 5;
            

            var layers = selectedAnimation.layers;
            for(int i = 0; i < layers.Count; i++){
                var group = layers[i].group;
                if(i == groupRects.Count) groupRects.Add(new GroupRects());

                Color offColor = group.color * Color.gray;

                Color moreOffColor = new Color(group.color.r/3, group.color.g/3, group.color.b/3 , 1f);

                
                float yPos = i == 0 ? xLayoutLine.yMax : xLayoutLine.yMax + ( i * 45 );
                var bodyRect = new Rect(xLayoutLine.xMin, yPos, width, height - 5);
                var settingsRect = new Rect(xLayoutLine.xMin, yPos, height - 5, height - 5);
                var bottomLine = new Rect(yLayoutLine.xMax, yPos + height - 5, timelineRect.width, lineHeight);
                groupRects[i] = new GroupRects(bodyRect, settingsRect, bottomLine);


                EditorGUI.DrawRect(bodyRect, offColor);
                EditorGUI.DrawRect(settingsRect, group.color);
                EditorGUI.DrawRect(bottomLine, blackColor);
                EditorGUI.DrawRect(groupRects[i].parting, moreOffColor);
                GUI.DrawTexture(settingsRect, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/settings.png", typeof(Texture2D)) );

                var tempColor = GUI.color;
                GUI.color = group.color * 1.5f; 
                EditorGUI.LabelField(new Rect(groupRects[i].settingsRect.xMax + 10, groupRects[i].settingsRect.yMin + lineHeight/2 + 5, width, 30), group.boxType);
                GUI.color = tempColor;

                if(SpriteAnimatorUtility.Button(groupRects[i].settingsRect, group.color)){
                    test = i;
                    settingsPopup.ShowAsContext();
                
                }


                // Set popup
                if(groupRects[i].bodyRect.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 1){
                    test = i;
                    settingsPopup.ShowAsContext();
                
                }

            }

        }



        private void SetLayerMenu(){
            var layers = selectedAnimation.layers;

            if(test == 0 )bringToUpGroup = false;
            else if(test == layers.Count-1) bringToDownGroup = false;

            if(bringToUpGroup){
                
                Debug.Log("girdin");
                
                var tempGroup = layers[test-1];
                layers[test-1] = layers[test];
                layers[test] = tempGroup;
                bringToUpGroup = false;
                Repaint();

            }
            else if(bringToDownGroup){
                var tempGroup = layers[test+1];

                layers[test+1] = layers[test];
                layers[test] = tempGroup;
                bringToDownGroup = false;
                Repaint();
            }
            else if(deleteGroup){
                layers.RemoveAt(test);

                deleteGroup = false;
                Repaint();
            }
            else if(changeGroupType){
                layers[test].group = changedType;
                changeGroupType = false;
            }

        }



        private void SetFrames(){
            var e = Event.current; 
            var layers = selectedAnimation.layers;
            selectFrameRect = new Rect[layers.Count, selectedAnimation.sprites.Count];
            int frameTextureSize = 16;

            
        
            for(int f = 0; f < selectedAnimation.sprites.Count; f ++){
                for(int i = 0; i < layers.Count; i++){

                    float width =  yLineRect[f].xMin - (yLineRect[f].xMin - greatestSize.x);
                    float height = groupRects[i].bottomLine.yMin - groupRects[i].bodyRect.yMin;

                    float yHalfPos = ( groupRects[i].bodyRect.yMin + height/2 ) - frameTextureSize/2;           
                    float xHalfPos = (yLineRect[f].xMin - greatestSize.x  + (yLineRect[f].xMin - (yLineRect[f].xMin - greatestSize.x))/2 ) - frameTextureSize/2;

                    frameRect[f] = new Rect(xHalfPos, yHalfPos, frameTextureSize, frameTextureSize);
                    selectFrameRect[i, f] = new Rect(yLineRect[f].xMin - greatestSize.x, groupRects[i].bottomLine.yMin - height, width, height);
                    GUI.DrawTexture(frameRect[f], singleFrameT);


                    if(e.type == EventType.MouseDown && e.button == 0 && selectFrameRect[i , f ].Contains(e.mousePosition)){

                        activeFrame = f;
                        activeSprite = selectedAnimation.sprites[activeFrame];
                        selectedGroup = i;
                    }
                    else if(e.type == EventType.MouseDrag && e.button == 0 && selectFrameRect[i, f].Contains(e.mousePosition)){

                    }



                    Rect topLeft = new Rect( yLineRect[activeFrame].xMin - greatestSize.x, groupRects[i].bodyRect.yMin, 15, 15 );
                    Rect topRight = new Rect( yLineRect[activeFrame].xMin - 15, groupRects[i].bodyRect.yMin, 15, 15 );
                    Rect bottomLeft = new Rect( yLineRect[activeFrame].xMin - greatestSize.x, groupRects[i].bottomLine.yMin - 15, 15, 15 );
                    Rect bottomRight = new Rect( yLineRect[activeFrame].xMin - 15, groupRects[i].bottomLine.yMin - 15, 15, 15 );
                    if(selectedGroup == i){

                        GUI.DrawTexture(topLeft, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Top Left.png", typeof(Texture2D)) );
                        GUI.DrawTexture(topRight, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Top Right.png", typeof(Texture2D)));
                        GUI.DrawTexture(bottomLeft, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Bottom Left.png", typeof(Texture2D)));
                        GUI.DrawTexture(bottomRight, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Bottom Right.png", typeof(Texture2D)));
                    }



                
                }
                
            }

            

        }

        #endregion



        private void Play(){

            timer += (editorDeltaTime * selectedAnimation.frameRate);
            if(timer >= 1f){
                timer -= 1f;
                activeFrame = (activeFrame + 1) % selectedAnimation.sprites.Count;
                this.activeSprite = selectedAnimation.sprites[activeFrame];
                if(selectedAnimation.sprites.Count > 1)playT = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Playing.png", typeof(Texture2D));
            }
            Repaint();
        }

        private void SetEditorDeltaTime(){

            if(lastTimeSinceStartup == 0f){
                lastTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
            }
            
            editorDeltaTime = (float)(EditorApplication.timeSinceStartup - lastTimeSinceStartup);
            lastTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
            
            
        }


        private void Update() {
            if(isPlaying) Play();
            else timer = 0;
            SetEditorDeltaTime();

            
        }

    }


    public struct GroupRects{

        public Rect bodyRect;
        public Rect settingsRect;
        public Rect bottomLine;
        public Rect parting{get{ return new Rect(bodyRect.xMin, bodyRect.yMax, bodyRect.width, 5); } set{}}
        

        

        public GroupRects(Rect bodyRect, Rect settingsRect, Rect bottomLine){
            this.bodyRect = bodyRect;
            this.settingsRect = settingsRect;
            this.bottomLine = bottomLine;



        }

    }
}

