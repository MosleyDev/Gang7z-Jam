using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Random = UnityEngine.Random;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEditor.AnimatedValues;
using System.IO;
using PixelAnimator;








namespace PixelAnimator{

    [CreateAssetMenu(menuName = "SpriteAnimation/ SpritePreferences")]

    public class SpriteAnimatorPreferences : ScriptableObject, ISearchWindowProvider {
        public List<SearchTreeEntry> selectedComponent;
        public List<SearchTreeEntry> searchList;
        public string[] packageDlls, generalDlls;

        // Dictionary e bak.
        [Space(30)]
        public List<Group> groups;
        [Space(10)]

        public List<HitboxProperty> hitboxProperties;
        public List<PropertyDataInfo> hitboxPropertyDatas;
        [Space(10)]
        
        public List<SpriteProperty> spriteProperties;
        public List<PropertyDataInfo> spritePropertyDatas;
        [Space(10)]

        public List<GUIContent> componentContent, framePropContent, hitboxPropContent;
        // serializable userData

        public int activeFramePropIndex;
        public int activeHitboxPropIndex;


        public ActiveProperty activeProperty;


        private void OnEnable() {
            SetComponentSearchWindow();

        }




        private void SetComponentSearchWindow(){
            searchList = new List<SearchTreeEntry>();

            searchList.Add(new SearchTreeGroupEntry( new GUIContent("Choose Component"),  0));
            // /Users/macpro/My test
            // Set package dlls
            packageDlls = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Library/ScriptAssemblies", "*.dll");
            foreach(var dll in packageDlls){
                Assembly package = Assembly.LoadFile(dll);
                foreach(var type in package.GetTypes()){
                    if(type.IsSubclassOf( typeof(Component) )){
                        
                        searchList.Add( new SearchTreeEntry(new GUIContent( type.Name )){level = 1} );

                    }
                }
            }

            // /Applications/Unity/Hub/Editor/2020.3.34f1/Unity.app/Contents
            // Set general Dlls
            generalDlls = Directory.GetFiles(EditorApplication.applicationContentsPath + @"/Managed/UnityEngine", "*dll");
            foreach(var dll in generalDlls){
                Assembly allComponent = Assembly.LoadFile(dll);
                foreach(var type in allComponent.GetTypes()){
                    if(type.IsSubclassOf( typeof(Component) )){

                        searchList.Add( new SearchTreeEntry( new GUIContent( type.Name ) ){level = 1} );

                    }
                }
            }
        } 
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context){ 
            
            return searchList;
        }

        
        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context){ 

            

            switch (activeProperty)
            {
                case ActiveProperty.Frame:
                    SetSpriteSearchWindowProperty(SearchTreeEntry);
                    break;
                case ActiveProperty.Hitbox:
                    SetHitboxSearchWindowProperty(SearchTreeEntry);
                    break;
            }

            return true;
            
        }

        // Selected anything pff
        public void SetSpriteSearchWindowProperty(SearchTreeEntry searchTreeEntry){
            spritePropertyDatas[activeFramePropIndex].selectedComponent = searchTreeEntry;

            for(int i = 0; i < spriteProperties.Count; i ++){


                foreach(var dlls in generalDlls){
                    Assembly generalComponent = Assembly.LoadFile(dlls);
                    foreach(var type in generalComponent.GetTypes()){

                        if(type.IsSubclassOf(typeof(Component))){
                            // Same Component Filter
                            if(type.Name == spritePropertyDatas[i].selectedComponent.name) {

                                spriteProperties[activeFramePropIndex].componentType = new SerializableSystemType(type);
                            }
                        }
                    }

                }
                foreach(var dlls in packageDlls){
                    Assembly packageComponent = Assembly.LoadFile(dlls);
                    foreach(var type in packageComponent.GetTypes()){

                        if(type.IsSubclassOf(typeof(Component))){
                            // Same Component Filter
                            if(type.Name == spritePropertyDatas[i].selectedComponent.name) {  

                                spriteProperties[activeFramePropIndex].componentType = new SerializableSystemType(type);
                            }

                        }
                    } 
                }
                
                
                
            }

            PropertyInfo[] propertyInfos = spriteProperties[activeFramePropIndex].componentType.SystemType.GetProperties
                                            ( BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty );

            spritePropertyDatas[activeFramePropIndex].propertyInfo.CreateItem();
            
            for(int i = 0; i < propertyInfos.Length; i ++){

                if( propertyInfos[i].CanWrite ){

                    spritePropertyDatas[activeFramePropIndex].propertyInfo.AddItem();
                    int index = spritePropertyDatas[activeFramePropIndex].propertyInfo.propertyName.Count -1;
                    
                    spritePropertyDatas[activeFramePropIndex].propertyInfo.propertyName[index] = propertyInfos[i].Name;
                    spritePropertyDatas[activeFramePropIndex].propertyInfo.propertyType[index] = new SerializableSystemType(propertyInfos[i].PropertyType);

                }

            }


        }
        
        public void SetHitboxSearchWindowProperty(SearchTreeEntry searchTreeEntry){
            hitboxPropertyDatas[activeHitboxPropIndex].selectedComponent = searchTreeEntry;
            var propData = hitboxPropertyDatas[activeHitboxPropIndex];

            for(int i = 0; i < hitboxProperties.Count; i ++){

                foreach(var dlls in generalDlls){
                    Assembly generalComponent = Assembly.LoadFile(dlls);
                    foreach(var type in generalComponent.GetTypes()){

                        if(type.IsSubclassOf(typeof(Component))){
                            if(type.Name == propData.selectedComponent.name) {


                                hitboxProperties[activeHitboxPropIndex].componentType = new SerializableSystemType(type);
                            }

                        }
                    }

                }
                foreach(var dlls in packageDlls){
                    Assembly packageComponent = Assembly.LoadFile(dlls);
                    foreach(var type in packageComponent.GetTypes()){

                        if(type.IsSubclassOf(typeof(Component))){
                            if(type.Name == propData.selectedComponent.name) {  

                                hitboxProperties[activeHitboxPropIndex].componentType = new SerializableSystemType(type);
                            }

                        }
                    } 
                }



            }

            PropertyInfo[] propertyInfos = hitboxProperties[activeHitboxPropIndex].componentType.SystemType.GetProperties
                                        ( BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty );

            propData.propertyInfo.CreateItem();

            for(int i = 0; i < propertyInfos.Length; i ++){
                if(propertyInfos[i].CanWrite){
                    propData.propertyInfo.AddItem();
                    int index = propData.propertyInfo.propertyName.Count - 1;

                    propData.propertyInfo.propertyName[index] = propertyInfos[i].Name;
                    propData.propertyInfo.propertyType[index] = new SerializableSystemType(propertyInfos[i].PropertyType);

                }
            }

        }

    }



    [CustomEditor(typeof(SpriteAnimatorPreferences))]
    public class SpriteAnimatorPreferencesEditor : Editor{

        #region Var
        string groupTypeName, spritePropertyName, hitboxPropertyName;
        Texture2D[] textures;

        private SpriteAnimatorPreferences preferences;
        SerializedObject so;
        SerializedProperty propGroups, propSpriteProperties, propHitboxProperties;
    

        public float windowSize { get {return EditorGUIUtility.currentViewWidth;} set{} }
        public List<SpriteProperty> spriteProperties{ get {return preferences.spriteProperties;} set{preferences.spriteProperties = value;} }
        public List<HitboxProperty> hitboxProperties{ get {return preferences.hitboxProperties;} set{preferences.hitboxProperties = value;} }
        
        private float lastTimeSinceStartup, editorDeltaTime, animationTimer;
        private AnimBool alredyExistWarning;
        private string changedGroupName;


        #endregion

        private void OnEnable() {


            textures = new Texture2D[4];
            textures[0] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/drop.png", typeof(Texture2D));
            textures[1] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/down.png", typeof(Texture2D));
            textures[2] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/-.png", typeof(Texture2D));
            textures[3] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/ok.png", typeof(Texture2D));

            preferences = (SpriteAnimatorPreferences)target;
            
            so = serializedObject;
            

            propGroups = so.FindProperty("groups");
            propSpriteProperties = so.FindProperty("spriteProperties");
            propHitboxProperties = so.FindProperty("hitboxProperties");


            EditorApplication.update += SetEditorDeltaTime;

            alredyExistWarning = new AnimBool(false);
            alredyExistWarning.valueChanged.AddListener(Repaint);


        }

        private void OnDisable() {
            EditorApplication.update -= SetEditorDeltaTime;
        }


        public override void OnInspectorGUI(){
            
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Hitbox Types", EditorStyles.boldLabel);

        

            using (new GUILayout.VerticalScope()) {
                so.Update();
                DrawGroupLabel();  // Classification.
                
                
                using(var check = new EditorGUI.ChangeCheckScope()){
                    DrawGroups(); // Draw how many groups there are.

                    GroupWarnings();
                    DrawAddGroupButton(); // Draw button for add groups.

                }
                
                GUILayout.Space(50);

                // DrawFrameProperties();
                // DrawAddFramePropButton();

                GUILayout.Space(50);
                DrawHitboxProperties();
                DrawAddHitboxPropButton();

            } 

            

            Repaint();
            GUILayout.Space(20);
        }


        #region Group


        private void DrawGroups(){

            if (preferences.groups.Count != 0) {

            
                for (int i = 0; i < propGroups.arraySize; i++) {

                    using (new GUILayout.HorizontalScope()) {
                        
                        var color = propGroups.GetArrayElementAtIndex(i).FindPropertyRelative("color");
                        var boxType = propGroups.GetArrayElementAtIndex(i).FindPropertyRelative("boxType");
                        var activeLayer = propGroups.GetArrayElementAtIndex(i).FindPropertyRelative("activeLayer");
                        var physicMaterial2D = propGroups.GetArrayElementAtIndex(i).FindPropertyRelative("physicMaterial");
                        var rounded = propGroups.GetArrayElementAtIndex(i).FindPropertyRelative("rounded");


                        EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(10));

                        EditorGUILayout.PropertyField(color, GUIContent.none, GUILayout.MaxWidth(windowSize / 10));
                        GUILayout.Space(20);


                        EditorGUILayout.TextField(boxType.stringValue, GUILayout.MaxWidth(windowSize/10));


                        GUILayout.Space(20);                    
                        
                        EditorGUILayout.PropertyField(activeLayer, GUIContent.none, GUILayout.MaxWidth(windowSize / 10));
                        EditorGUILayout.LabelField(LayerMask.LayerToName(activeLayer.intValue), GUILayout.MaxWidth(windowSize / 10));
                                
                        GUILayout.Space(20);

                        EditorGUILayout.PropertyField(physicMaterial2D, GUIContent.none, GUILayout.MaxWidth(windowSize / 10));

                        GUILayout.Space(30);
                        EditorGUILayout.PropertyField(rounded, GUIContent.none, GUILayout.MaxWidth(windowSize/10));
                                        

                        GUILayoutOption buttonSizeX = GUILayout.MaxWidth(20);
                        GUILayoutOption buttonSizeY = GUILayout.MaxHeight(20);

                        if (GUILayout.Button(textures[0], buttonSizeX, buttonSizeY)) {
                            if (i != 0) {
                                var tempGroup = preferences.groups[i-1];
                                preferences.groups[i - 1] = preferences.groups[i];
                                preferences.groups[i] = tempGroup;

                            }
                            SpriteAnimatorDataProvider.Instance.isChangedGroupVarible = true;
                        }
                        else if (GUILayout.Button(textures[1], buttonSizeX, buttonSizeY)) {
                            if (i != preferences.groups.Count-1) {
                                
                                var tempGroup = preferences.groups[i + 1];
                                preferences.groups[i + 1] = preferences.groups[i];
                                preferences.groups[i] = tempGroup;
    
                            }
                            SpriteAnimatorDataProvider.Instance.isChangedGroupVarible = true;
                        }
                        else if (GUILayout.Button(textures[2], buttonSizeX, buttonSizeY)) {


                            propGroups.DeleteArrayElementAtIndex(i);
                            propGroups.serializedObject.ApplyModifiedProperties();
                            SpriteAnimatorDataProvider.Instance.isChangedGroupVarible = true;
                            
                        }
                    
                        so.ApplyModifiedProperties();

                    }
                }


            }

            
        }
        private void DrawAddGroupButton(){  

            using(new GUILayout.HorizontalScope()){
                
                string defultText = "New Box Type";

                groupTypeName = groupTypeName == null ? defultText : groupTypeName;
                groupTypeName = EditorGUILayout.TextField(groupTypeName, GUILayout.MaxWidth(windowSize));

                bool alreadyExist = preferences.groups.Any(g => g.boxType == groupTypeName);
                                
                if(GUILayout.Button("+ Box Type", GUILayout.MaxWidth(windowSize))){
                    animationTimer = 2;
                    
                    if (groupTypeName != defultText  && !alreadyExist){
                        SpriteAnimatorDataProvider.Instance.isChangedGroupVarible = true;
                        AddNewGroup();

                        groupTypeName = defultText;
                    }

                    if(alreadyExist){
                        alredyExistWarning.target = true;
                    }


                
                }
                if(alredyExistWarning.target){
                    animationTimer -= editorDeltaTime;
                    if(animationTimer <= 0 ) alredyExistWarning.target = false;
                }
                GUILayout.Space(10);
                
            }
            
        }

        private void GroupWarnings(){

            if(EditorGUILayout.BeginFadeGroup(alredyExistWarning.faded)){
                EditorGUI.indentLevel ++;
                GUIStyle labelStyle = new GUIStyle();
                labelStyle = EditorStyles.helpBox;
                labelStyle.fontSize = 15;
                EditorGUILayout.LabelField(new GUIContent("Alredy Exist", (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SpriteAnimator/Editor Default Resources/Warning.png", typeof(Texture2D))),
                                labelStyle, GUILayout.MaxWidth(windowSize/3), GUILayout.MinHeight(30));
                EditorGUI.indentLevel --;



            }
            EditorGUILayout.EndFadeGroup();
        }


        private void AddNewGroup(){
            preferences.groups.Add(new Group());

            int activeIndex = preferences.groups.Count -1;
            
            
            so.Update();
            preferences.groups[activeIndex].boxType = groupTypeName;
            preferences.groups[activeIndex].groupID = (int)Int64.Parse(String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000));

            so.ApplyModifiedProperties();
                        
        }

        private void DrawGroupLabel(){
            using(new GUILayout.HorizontalScope()){
                var maxWidth = GUILayout.MaxWidth(windowSize/6);
                EditorGUILayout.LabelField("Color", GUILayout.MaxWidth(windowSize/7));
                
                
                EditorGUILayout.LabelField("Box Type", maxWidth);
                
                EditorGUILayout.LabelField("Active Layer", maxWidth);
                EditorGUILayout.LabelField("Physcis Material", maxWidth);
                EditorGUILayout.LabelField("Rounded", maxWidth);

                
            }
        }
        
        #endregion

        #region Frame Properties
        private void DrawFrameProperties(){
            // Tips
            string manuelTip = "Type the name of the object from which you will receive the Component from the Animator.";
            string componentHelp = "Please choose only those components that will work for you. Otherwise, problems may arise.";

            EditorGUILayout.LabelField("Frame Properties", EditorStyles.boldLabel);
            using(new GUILayout.HorizontalScope()){
                // Set Label
                EditorGUILayout.LabelField("Get Component Way", GUILayout.MaxWidth(windowSize/5));
                EditorGUILayout.LabelField("Which Component", GUILayout.MaxWidth(windowSize/6));
                EditorGUILayout.LabelField("Property Name", GUILayout.MaxWidth(windowSize/5));     

            }
            
            for(int i = 0; i < spriteProperties.Count; i ++){


                // get array element
                var name = propSpriteProperties.GetArrayElementAtIndex(i).FindPropertyRelative("name");
                var componentType = propSpriteProperties.GetArrayElementAtIndex(i).FindPropertyRelative("componentType");
                var componentWay = propSpriteProperties.GetArrayElementAtIndex(i).FindPropertyRelative("componentWay");
                var selectDataMenu = propSpriteProperties.GetArrayElementAtIndex(i).FindPropertyRelative("selectData");

                using(new GUILayout.VerticalScope()){
                    using(new GUILayout.HorizontalScope()){

                        // Component Way Select
                        if(componentWay.enumValueIndex == 2){
                            EditorGUILayout.LabelField(new GUIContent("Info", manuelTip), EditorStyles.toolbar,GUILayout.MaxWidth(30));
                            EditorGUILayout.PropertyField(componentWay, GUIContent.none, GUILayout.MaxWidth(windowSize/6));

                        } 
                        else EditorGUILayout.PropertyField(componentWay, GUIContent.none, GUILayout.MaxWidth(windowSize/5));
                            
                        
                        

                        // Component Select
                        if(preferences.spritePropertyDatas.Count > 0){

                            var tempContent = new GUIContent(preferences.selectedComponent[i].name);
                
                            preferences.componentContent[i] = tempContent; 
                            //Bilmiyorum ne oluyor
                            
                            spriteProperties[preferences.activeFramePropIndex].selectedData = new GenericMenu();

                            for(int x = 0; x < preferences.spritePropertyDatas[i].propertyInfo.propertyName.Count; x++){


                                var propType = preferences.spritePropertyDatas[i].propertyInfo.propertyType[x];
                                var propName = preferences.spritePropertyDatas[i].propertyInfo.propertyName[x];
                                string itemName = propName + @"  \  " + propType.Name;

                                spriteProperties[i].selectedData.AddItem(new GUIContent(itemName), false, () => {

                                    
                                    preferences.framePropContent[preferences.activeFramePropIndex].text = itemName;
                                    spriteProperties[preferences.activeFramePropIndex].dataType = propType;
                                    spriteProperties[preferences.activeFramePropIndex].dataName = propName;

                                });

                                
                            }

                            // Search Window
                            if(GUILayout.Button(preferences.componentContent[i], EditorStyles.popup, GUILayout.MaxWidth(windowSize/7))){
                                
                                preferences.activeFramePropIndex = i;
                                preferences.activeProperty = ActiveProperty.Frame;
                                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), preferences);

                            }

                        }

                        // Data Select
                        if(GUILayout.Button(preferences.framePropContent[i], EditorStyles.popup, GUILayout.MaxWidth(windowSize/7))){
                            preferences.activeFramePropIndex = i;
                            preferences.activeProperty = ActiveProperty.Frame;

                            preferences.spriteProperties[i].selectedData.ShowAsContext();
                        }
                        
                        EditorGUILayout.PropertyField(name, GUIContent.none, GUILayout.MaxWidth(windowSize/8));



                        // Up-Down-Remove buttons
                        GUILayoutOption buttonSizeX = GUILayout.MaxWidth(20);
                        GUILayoutOption buttonSizeY = GUILayout.MaxHeight(20);

                        if (GUILayout.Button(textures[0], buttonSizeX, buttonSizeY)) {
                            preferences.activeProperty = ActiveProperty.Frame;
                            if (i != 0) {

                                var tempGroup = spriteProperties[i-1];
                                var tempContent = preferences.componentContent[i-1];
                                spriteProperties[i - 1] = spriteProperties[i];
                                preferences.componentContent[i - 1] = preferences.componentContent[i];
                                preferences.componentContent[i] = tempContent;
                                spriteProperties[i] = tempGroup;

                            }
                        }
                        else if (GUILayout.Button(textures[1], buttonSizeX, buttonSizeY)) {
                            preferences.activeProperty = ActiveProperty.Frame;
                            if (i != preferences.spriteProperties.Count-1) {
                                
                                var tempGroup = spriteProperties[i + 1];
                                var tempContent = preferences.componentContent[i + 1];
                                spriteProperties[i + 1] = spriteProperties[i];
                                preferences.componentContent[i + 1] = preferences.componentContent[i];
                                preferences.componentContent[i] = tempContent;
                                spriteProperties[i] = tempGroup;

                            }
                        }
                        else if (GUILayout.Button(textures[2], buttonSizeX, buttonSizeY)) {
                            preferences.activeProperty = ActiveProperty.Frame;
                            spriteProperties.RemoveAt(i);
                            preferences.selectedComponent.RemoveAt(i);
                            preferences.spritePropertyDatas.RemoveAt(i);
                            preferences.componentContent.RemoveAt(i);
                            preferences.framePropContent.RemoveAt(i);
                        
                        }


                    }

                    so.ApplyModifiedProperties();

                }

                

            }

            EditorGUILayout.HelpBox(componentHelp, MessageType.Info);  



        }

        private void DrawAddFramePropButton(){
            
            using(new GUILayout.HorizontalScope()){
                string defultText = "New Sprite Property ";

                spritePropertyName = spritePropertyName == null ? defultText : spritePropertyName;
                spritePropertyName = EditorGUILayout.TextField(spritePropertyName, GUILayout.MaxWidth(windowSize));
            

                if(GUILayout.Button("+" + defultText, GUILayout.MaxWidth(windowSize))){
                    
                    spriteProperties.Add(new SpriteProperty());
                    preferences.activeProperty = ActiveProperty.Frame;
                    


                    if(preferences.componentContent == null) preferences.componentContent = new List<GUIContent>();
                    preferences.componentContent.Add(new GUIContent("Select Component"));


                    if(preferences.framePropContent == null) preferences.framePropContent = new List<GUIContent>();
                    preferences.framePropContent.Add(new GUIContent("Select Data"));

                    
                    if(preferences.selectedComponent == null)preferences.selectedComponent = new List<SearchTreeEntry>();
                    preferences.selectedComponent.Add(new SearchTreeEntry(new GUIContent()));
                    
                    if(preferences.spritePropertyDatas == null) preferences.spritePropertyDatas = new List<PropertyDataInfo>();
                    preferences.spritePropertyDatas.Add(new PropertyDataInfo());
        
                    Debug.Log("sadasd");
                    int index = spriteProperties.Count-1;
                    spriteProperties[index].propID = (int)Int64.Parse(String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000));
                    spriteProperties[index].name = spritePropertyName;
                    preferences.activeFramePropIndex = index;

                    spritePropertyName = defultText;

                    
                }


            }

        }


        #endregion

        #region Hitbox Properties
        private void DrawHitboxProperties(){
            // Tips
            string manuelTip = "Type the name of the object from which you will receive the Component from the Animator.";
            string componentHelp = "Please choose only those components that will work for you. Otherwise, problems may arise.";

            EditorGUILayout.LabelField("Hitbox Properties", EditorStyles.boldLabel);
            using(new GUILayout.HorizontalScope()){
                // Set Label
                EditorGUILayout.LabelField("Get Component Way", GUILayout.MaxWidth(windowSize/5));
                EditorGUILayout.LabelField("Which Component", GUILayout.MaxWidth(windowSize/6));
                EditorGUILayout.LabelField("Property Name", GUILayout.MaxWidth(windowSize/5));     

            }

            for(int i = 0; i < hitboxProperties.Count; i ++){

                //get array element
                var name = propHitboxProperties.GetArrayElementAtIndex(i).FindPropertyRelative("name");
                var componentType = propHitboxProperties.GetArrayElementAtIndex(i).FindPropertyRelative("componentType");
                var componentWay = propHitboxProperties.GetArrayElementAtIndex(i).FindPropertyRelative("componentWay");
                var selectDataMenu = propHitboxProperties.GetArrayElementAtIndex(i).FindPropertyRelative("selectData");

                using(new GUILayout.VerticalScope()){
                    using(new GUILayout.HorizontalScope()){

                        // Component Way Select
                        if(componentWay.enumValueIndex == 2){
                            EditorGUILayout.LabelField(new GUIContent("Info", manuelTip), EditorStyles.toolbar,GUILayout.MaxWidth(30));
                            EditorGUILayout.PropertyField(componentWay, GUIContent.none, GUILayout.MaxWidth(windowSize/6));

                        } 
                        else EditorGUILayout.PropertyField(componentWay, GUIContent.none, GUILayout.MaxWidth(windowSize/5));
                            
                        
                        
                        // Component Select
                        var tempContent = new GUIContent(preferences.hitboxPropertyDatas[i].selectedComponent.name);

                    
                        // Search Window
                        if(GUILayout.Button(tempContent, EditorStyles.popup, GUILayout.MaxWidth(windowSize/7))){
                            preferences.activeHitboxPropIndex = i;
                            preferences.activeProperty = ActiveProperty.Hitbox;
                            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), preferences);

                        
                        }

                        
                        var content = preferences.hitboxProperties[i].dataName + preferences.hitboxProperties[i].dataType.Name;
                        // Data Select
                        if(GUILayout.Button(content, EditorStyles.popup, GUILayout.MaxWidth(windowSize/7))){
                            preferences.activeHitboxPropIndex = i;
                            preferences.activeProperty = ActiveProperty.Hitbox;

                            preferences.hitboxPropertyDatas[preferences.activeHitboxPropIndex].selectData = new GenericMenu();
                            var propInfo = preferences.hitboxPropertyDatas[i].propertyInfo;
                            
                            for(int x = 0; x < propInfo.propertyName.Count; x++){


                                var propType = propInfo.propertyType[x];
                                var propName = propInfo.propertyName[x];
                                string itemName = propName + @"  \  " + propType.Name;

                                preferences.hitboxPropertyDatas[i].selectData.AddItem(new GUIContent(itemName), false, () => {

                                    preferences.hitboxPropContent[preferences.activeHitboxPropIndex].text = itemName;
                                    hitboxProperties[preferences.activeHitboxPropIndex].dataType = propType;
                                    hitboxProperties[preferences.activeHitboxPropIndex].dataName = propName;

                                });

                            
                            }

                            preferences.hitboxPropertyDatas[i].selectData.ShowAsContext();

                        }
                        
                        EditorGUILayout.PropertyField(name, GUIContent.none, GUILayout.MaxWidth(windowSize/8));
                        EditorGUILayout.IntField(hitboxProperties[i].propID, GUILayout.MaxWidth(150));


                        // Up-Down-Remove buttons
                        GUILayoutOption buttonSizeX = GUILayout.MaxWidth(20);
                        GUILayoutOption buttonSizeY = GUILayout.MaxHeight(20);

                        if (GUILayout.Button(textures[0], buttonSizeX, buttonSizeY)) {
                            preferences.activeProperty = ActiveProperty.Hitbox;
                            if (i != 0) {
                                


                            }
                        }
                        else if (GUILayout.Button(textures[1], buttonSizeX, buttonSizeY)) {
                            
                            preferences.activeProperty = ActiveProperty.Hitbox;
                            if (i != preferences.spriteProperties.Count-1) {
                                


                            }
                        }
                        else if (GUILayout.Button(textures[2], buttonSizeX, buttonSizeY)) {
                            
                            preferences.activeProperty = ActiveProperty.Hitbox;
                            hitboxProperties.RemoveAt(i);
                            preferences.hitboxPropertyDatas.RemoveAt(i);
                            preferences.hitboxPropContent.RemoveAt(i);
                        
                        }


                    }

                    so.ApplyModifiedProperties();

                }

            }
                EditorGUILayout.HelpBox(componentHelp, MessageType.Info);  

            




        }
        private void DrawAddHitboxPropButton(){
            
            using(new GUILayout.HorizontalScope()){
                string defultText = "New Hitbox Property ";

                hitboxPropertyName = hitboxPropertyName == null ? defultText : hitboxPropertyName;
                hitboxPropertyName = EditorGUILayout.TextField(hitboxPropertyName, GUILayout.MaxWidth(windowSize));
            

                if(GUILayout.Button("+" + defultText, GUILayout.MaxWidth(windowSize))){
                    preferences.activeProperty = ActiveProperty.Hitbox;
                    hitboxProperties.Add(new HitboxProperty());
                    

                    if(preferences.hitboxPropertyDatas == null) preferences.hitboxPropertyDatas = new List<PropertyDataInfo>();
                    preferences.hitboxPropertyDatas.Add(new PropertyDataInfo());

                    if(preferences.hitboxPropContent == null) preferences.hitboxPropContent = new List<GUIContent>();
                    preferences.hitboxPropContent.Add(new GUIContent());

                    int index = hitboxProperties.Count - 1;
                    hitboxProperties[index].propID = (int)Int64.Parse(String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000));
                    hitboxProperties[index].name = hitboxPropertyName;
                    hitboxPropertyName = defultText;

                    
                }


            }

        }

        #endregion 

        private void SetEditorDeltaTime(){

            if(lastTimeSinceStartup == 0f){
                lastTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
            }
            
            editorDeltaTime = (float)(EditorApplication.timeSinceStartup - lastTimeSinceStartup);
            lastTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
            
            
        }

    }
    // Serializable PropertyInfo
    [Serializable]
    public class SerializablePropertyInfo{

        public List<string> propertyName = new List<string>();
        public List<SerializableSystemType> propertyType = new List<SerializableSystemType>();



        public void AddItem(){
            propertyName.Add( null);
            propertyType.Add(null);

        }

        public void CreateItem(){
            propertyName = new List<string>();
            propertyType = new List<SerializableSystemType>();

        }



    }
    public enum ActiveProperty { Frame, Hitbox }


    [Serializable]
    public class PropertyDataInfo{
        public SearchTreeEntry selectedComponent;
        public SerializablePropertyInfo propertyInfo;
        public GenericMenu selectData;



        public PropertyDataInfo(){
            selectedComponent = new SearchTreeEntry(new GUIContent());
            propertyInfo = new SerializablePropertyInfo();
            selectData = new GenericMenu();


        }   

    }




}