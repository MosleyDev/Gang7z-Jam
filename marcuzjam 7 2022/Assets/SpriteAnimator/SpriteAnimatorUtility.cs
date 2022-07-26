using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;



namespace PixelAnimator{


    public class SpriteAnimatorUtility{

        public static GenericMenu boxTypePopup;
        public static SpriteAnimation spriteAnim;
        



        public static void DrawBox(Layer layer, Color rectColor, int index, bool rounded){
            var e = Event.current;
            var rect = layer.frames[index].hitboxRect;
            var frame = layer.frames[index];
            


            float size = 1.5f;
            float bigSize = 4;
            Rect rTopLeft = new Rect(rect.xMin - size/2, rect.yMin - size/2, size, size);
            Rect rBigTopLeft = new Rect(rect.xMin - bigSize/2, rect.yMin - bigSize/2, bigSize, bigSize);
            Rect rTopCenter = new Rect((rect.xMin + rect.width/2) - size/2, rect.yMin - size/2, size, size);
            Rect rTopRight = new Rect(rect.xMax - size/2, rect.yMin - size/2, size, size);
            Rect rRightCenter = new Rect(rect.xMax - size/2, rect.yMin + (rect.yMax - rect.yMin)/2 - size/2, size, size);
            Rect rBottomRight = new Rect(rect.xMax - size/2, rect.yMax - size/2, size, size);
            Rect rBottomCenter = new Rect((rect.xMin + rect.width/2) - size/2, rect.yMax - size/2, size, size);
            Rect rBottomLeft = new Rect(rect.xMin - size/2, rect.yMax - size/2, size, size);
            Rect rLeftCenter = new Rect(rect.xMin - size/2, rect.yMin + (rect.yMax - rect.yMin)/2 - size/2, size, size);

            if(layer.reSizeBox){


                if(e.button == 0 && e.type == EventType.MouseDown){
                    if(rTopLeft.Contains(e.mousePosition))
                        layer.onHandleTopleft = true;
                    else if(rTopCenter.Contains(e.mousePosition))
                        layer.onHandleTopCenter = true;
                    else if(rTopRight.Contains(e.mousePosition))
                        layer.onHandleTopRight = true;
                    else if(rRightCenter.Contains(e.mousePosition))
                        layer.onHandleRightCenter = true;
                    else if(rBottomRight.Contains(e.mousePosition))
                        layer.onHandleBottomRight = true;
                    else if(rBottomCenter.Contains(e.mousePosition))
                        layer.onHandleBottomCenter = true;
                    else if(rBottomLeft.Contains(e.mousePosition))
                        layer.onHandleBottomLeft = true;
                    else if(rLeftCenter.Contains(e.mousePosition))
                        layer.onHandleLeftCenter = true;
                }



                if(layer.onHandleTopleft){
                    if(rounded){
                        
                        frame.hitboxRect.xMin = (int)e.mousePosition.x;
                        frame.hitboxRect.yMin = (int)e.mousePosition.y;
                        
                    }else{
                        frame.hitboxRect.xMin = e.mousePosition.x;
                        frame.hitboxRect.yMin = e.mousePosition.y;
                        
                    }
                    
                }
                else if(layer.onHandleTopCenter){
                    if(rounded){
                        frame.hitboxRect.yMin = (int)e.mousePosition.y;
                    }else{
                        frame.hitboxRect.yMin = e.mousePosition.y;

                    }
                }
                else if(layer.onHandleTopRight){
                    if(rounded){

                        frame.hitboxRect.xMax = (int)e.mousePosition.x;
                        frame.hitboxRect.yMin = (int)e.mousePosition.y;
                        
                    }else{

                        frame.hitboxRect.xMax = e.mousePosition.x;
                        frame.hitboxRect.yMin = e.mousePosition.y;
                        
                    }
                }
                else if(layer.onHandleRightCenter){
                    if(rounded){
                        frame.hitboxRect.xMax = (int)e.mousePosition.x;

                    }else{

                        frame.hitboxRect.xMax = e.mousePosition.x;
                    }
                }
                else if(layer.onHandleBottomRight){
                    if(rounded){

                        frame.hitboxRect.xMax = (int)e.mousePosition.x;
                        frame.hitboxRect.yMax = (int)e.mousePosition.y;
                    
                    }else{

                        frame.hitboxRect.xMax = e.mousePosition.x;
                        frame.hitboxRect.yMax = e.mousePosition.y;
                    
                    }
                }
                else if(layer.onHandleBottomCenter){
                    if(rounded){

                        frame.hitboxRect.yMax = (int)e.mousePosition.y;
                    }else{

                        frame.hitboxRect.yMax = e.mousePosition.y;
                    }
                }
                else if(layer.onHandleBottomLeft){
                    if(rounded){

                        frame.hitboxRect.xMin = (int)e.mousePosition.x;
                        frame.hitboxRect.yMax = (int)e.mousePosition.y;
                        
                    }else{
                        
                        frame.hitboxRect.xMin = e.mousePosition.x;
                        frame.hitboxRect.yMax = e.mousePosition.y;
                        
                    }
                }
                else if(layer.onHandleLeftCenter){
                    if(rounded){

                        frame.hitboxRect.xMin = (int)e.mousePosition.x;
                    }else{
                        frame.hitboxRect.xMin = e.mousePosition.x;

                    }
                }
                else if(rect.Contains(e.mousePosition) && e.button == 0 && e.type == EventType.MouseDrag){

                    frame.hitboxRect.position += e.delta;
                    
                }
                var smallRect = new Rect(rect.xMin + ((rect.xMax - rect.xMin)/1.5f)/4 , rect.yMin + ((rect.yMax - rect.yMin)/1.5f)/4, rect.size.x/1.5f, rect.size.y/1.5f);
                frame.hitboxRect.position = new Vector2(Mathf.RoundToInt(frame.hitboxRect.position.x), Mathf.RoundToInt(frame.hitboxRect.position.y));
                frame.hitboxRect.size = new Vector2(Mathf.RoundToInt(frame.hitboxRect.size.x), Mathf.RoundToInt(frame.hitboxRect.size.y));


                EditorGUI.DrawRect( rTopLeft , rectColor );
                EditorGUI.DrawRect( rTopCenter , rectColor );
                EditorGUI.DrawRect( rTopRight , rectColor );
                EditorGUI.DrawRect( rRightCenter , rectColor );
                EditorGUI.DrawRect( rBottomRight , rectColor );
                EditorGUI.DrawRect( rBottomCenter , rectColor );
                EditorGUI.DrawRect( rBottomLeft , rectColor );
                EditorGUI.DrawRect( rLeftCenter , rectColor );

            }
            if(e.type == EventType.MouseUp){
                layer.onHandleTopleft = false;
                layer.onHandleTopCenter = false;
                layer.onHandleTopRight = false;
                layer.onHandleRightCenter = false;
                layer.onHandleBottomRight = false;
                layer.onHandleBottomCenter = false;
                layer.onHandleBottomLeft = false;
                layer.onHandleLeftCenter = false;
            }


            var halfColor = layer.reSizeBox == true ? new Color(rectColor.r, rectColor.g, rectColor.b, 0.2f) : Color.clear;    

            Handles.DrawSolidRectangleWithOutline( rect, halfColor, rectColor );

            EditorGUIUtility.AddCursorRect(rTopLeft, MouseCursor.ResizeUpLeft);
            EditorGUIUtility.AddCursorRect(rTopCenter, MouseCursor.ResizeVertical);
            EditorGUIUtility.AddCursorRect(rTopRight, MouseCursor.ResizeUpRight);
            EditorGUIUtility.AddCursorRect(rRightCenter, MouseCursor.ResizeHorizontal);
            EditorGUIUtility.AddCursorRect(rBottomRight, MouseCursor.ResizeUpLeft);
            EditorGUIUtility.AddCursorRect(rBottomCenter, MouseCursor.ResizeHorizontal);
            EditorGUIUtility.AddCursorRect(rBottomRight, MouseCursor.ResizeUpRight);
            EditorGUIUtility.AddCursorRect(rLeftCenter, MouseCursor.ResizeHorizontal);

        }




        public static bool Button(Texture2D image, Rect rect){
            var e = Event.current;
            GUI.DrawTexture(rect, image);
            if(rect.Contains(e.mousePosition)){
                EditorGUI.DrawRect(rect, new Color(255, 255, 255, 0.2f)); 
                if(e.button == 0 && e.type == EventType.MouseDown){
                    return true;
                }
            }
            return false;
        }

        public static bool Button(Rect rect, Color color){
            var e = Event.current;
            EditorGUI.DrawRect(rect, color);
            if(rect.Contains(e.mousePosition)){
                EditorGUI.DrawRect(rect, new Color(255, 255, 255, 0.2f)); 
                if(e.button == 0 && e.type == EventType.MouseDown){
                    return true;
                }
            }
            return false;
        }

        public static bool Button(Texture2D defaultImg, Texture2D onMouse, Rect rect){
            var e = Event.current;
            if(rect.Contains(e.mousePosition)){
                GUI.DrawTexture(rect, onMouse);
                EditorGUI.DrawRect(rect, new Color(255, 255, 255, 0.2f)); 
                if(e.button == 0 && e.type == EventType.MouseDown){
                    return true;
                }
            }else{
                GUI.DrawTexture(rect, defaultImg);

            }
            return false;
        }



        public static void DrawProperty(Type type){
            using(new EditorGUI.DisabledGroupScope(true))
            {
                var obj = Activator.CreateInstance(type);
                if(type == typeof(int)){
                    EditorGUILayout.IntField((int)obj);
                }
                else if(type == typeof(string)){
                    EditorGUILayout.TextField((string)obj);
                }
                else if(type == typeof(bool)){
                    EditorGUILayout.Toggle((bool)obj);
                }
                else if(type == typeof(float)){
                    EditorGUILayout.FloatField((float)obj);
                }
                else if(type == typeof(long)){
                    EditorGUILayout.LongField((long)obj);
                }
                else if(type == typeof(double)){
                    EditorGUILayout.DoubleField((double)obj);
                }
                else if(type == typeof(Rect)){
                    EditorGUILayout.RectField((Rect)obj);
                }
                else if(type == typeof(RectInt)){
                    EditorGUILayout.RectIntField((RectInt)obj);
                }
                else if(type == typeof(Color)){
                    EditorGUILayout.ColorField((Color)obj);
                }
                else if(type == typeof(AnimationCurve)){
                    EditorGUILayout.CurveField((AnimationCurve)obj);
                }
                else if(type == typeof(Bounds)){
                    EditorGUILayout.BoundsField((Bounds)obj);
                }
                else if(type == typeof(BoundsInt)){
                    EditorGUILayout.BoundsIntField((BoundsInt)obj);
                }
                else if(type == typeof(Vector2)){
                    EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)obj, GUILayout.MaxWidth(130));
                }
                else if(type == typeof(Vector3)){
                    EditorGUILayout.Vector3Field(GUIContent.none, (Vector3) obj, GUILayout.MaxWidth(90));
                }
                else if(type == typeof(Vector4)){
                    EditorGUILayout.Vector4Field(GUIContent.none, (Vector4)obj, GUILayout.MaxWidth(80));
                }
                else if(type == typeof(Vector2Int)){
                    EditorGUILayout.Vector2IntField(GUIContent.none, (Vector2Int) obj, GUILayout.MaxWidth(130));
                }
                else if(type == typeof(Vector3Int)){
                    EditorGUILayout.Vector3IntField(GUIContent.none, (Vector3Int) obj, GUILayout.MaxWidth(90));
                }
                else if(type.IsSubclassOf(typeof(Component))){
                    EditorGUILayout.ObjectField((Component)obj, type, false);
                }
                else if(type == typeof(Gradient)){
                    EditorGUILayout.GradientField((Gradient)obj);
                }
                else {
                    Debug.Log("Bisey yok alooo");
                }
            }


        }// Serilize etmeye bak iste yaw.


        public static void DrawProperty(SerializedProperty serializedProperty, Type type, int iD, HitboxData hitboxData){
            SerializedProperty prop = null;
            int index = -1;
            using(new EditorGUILayout.HorizontalScope()){

                if(type == typeof(int)){
                    bool alreadyExist = hitboxData.intProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{
                        index = hitboxData.intProps.IndexOf(hitboxData.intProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("intProps");
                    }

                }
                else if(type == typeof(string)){
                    
                    bool alreadyExist = hitboxData.stringProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.stringProps.IndexOf(hitboxData.stringProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("stringProps");
                    }

                }
                else if(type == typeof(bool)){  
                    bool alreadyExist = hitboxData.boolProps.Any(x => x.iD == iD);
                    if(!alreadyExist){
                        Debug.Log("saaaa");
                        DrawProperty(type);
                    }else{

                        index = hitboxData.boolProps.IndexOf(hitboxData.boolProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("boolProps");
                    }


                }
                else if(type == typeof(float)){
                    bool alreadyExist = hitboxData.floatProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.floatProps.IndexOf(hitboxData.floatProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("floatProps");
                    }
                }
                else if(type == typeof(long)){
                    bool alreadyExist = hitboxData.longProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.longProps.IndexOf(hitboxData.longProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("longProps");
                    }

                }
                else if(type == typeof(double)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.doubleProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.doubleProps.IndexOf(hitboxData.doubleProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("doubleProps");
                    }

                }
                else if(type == typeof(Rect)){
                    bool alreadyExist = false; 
                    alreadyExist = hitboxData.rectProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.rectProps.IndexOf(hitboxData.rectProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("rectProps");
                    }

                }
                else if(type == typeof(RectInt)){
                    bool alreadyExist = false; 
                    alreadyExist = hitboxData.rectIntProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.rectIntProps.IndexOf(hitboxData.rectIntProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("rectIntProps");
                    }

                }
                else if(type == typeof(Color)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.colorProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.colorProps.IndexOf(hitboxData.colorProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("colorProps");
                    }

                }
                else if(type == typeof(AnimationCurve)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.animationCurveProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.animationCurveProps.IndexOf(hitboxData.animationCurveProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("animationCurveProps");
                    }

                }
                else if(type == typeof(Bounds)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.boundsProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.boundsProps.IndexOf(hitboxData.boundsProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("boundsProps");
                    }

                }
                else if(type == typeof(BoundsInt)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.boundsIntProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.boundsIntProps.IndexOf(hitboxData.boundsIntProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("boundsIntProps");
                    }

                }
                else if(type == typeof(Vector2)){
                    bool alreadyExist = hitboxData.vector2Props.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{
                        index = hitboxData.vector2Props.IndexOf(hitboxData.vector2Props.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("vector2Props");

                    }

                }
                else if(type == typeof(Vector3)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.vector3Props.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{
                        index = hitboxData.vector3Props.IndexOf(hitboxData.vector3Props.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("vector3Props");

                    }
                }
                else if(type == typeof(Vector4)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.vector4Props.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.vector4Props.IndexOf(hitboxData.vector4Props.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("vector4Props");
                    }

                }
                else if(type == typeof(Vector2Int)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.vector2IntProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.vector2IntProps.IndexOf(hitboxData.vector2IntProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("vector2IntProps");
                    }

                }
                else if(type == typeof(Vector3Int)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.vector3IntProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        prop = serializedProperty.FindPropertyRelative("vector3IntProps");
                    }

                }
                else if(type.IsSubclassOf(typeof(UnityEngine.Object))){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.unityObjectProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{
                        
                        index = hitboxData.unityObjectProps.IndexOf(hitboxData.unityObjectProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("unityObjectProps");
                    }

                }
                else if(type == typeof(Gradient)){
                    bool alreadyExist = false;
                    alreadyExist = hitboxData.gradientProps.Any(x => x.iD == iD);
                    if(!alreadyExist)
                        DrawProperty(type);
                    else{

                        index = hitboxData.gradientProps.IndexOf(hitboxData.gradientProps.First(x => x.iD == iD));
                        prop = serializedProperty.FindPropertyRelative("gradientProps");
                    }

                }

                else {
                    prop = null;
                    Debug.Log("Bisey yok alooo");
                }
            }
                serializedProperty.serializedObject.Update();
                if(prop != null && index >= 0){
                    Debug.Log(prop.arraySize);
                    EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(index).FindPropertyRelative("data"), GUIContent.none, GUILayout.MaxWidth(140));
                    
                }
            serializedProperty.serializedObject.ApplyModifiedProperties();

        }




        public static void AddProperty(HitboxData hitboxData, HitboxProperty prop){
                var obj = Activator.CreateInstance(prop.dataType.SystemType);

                if(obj.GetType() == typeof(int)){
                    
                    if(!hitboxData.intProps.Any(x => x.iD == prop.propID))
                        hitboxData.intProps.Add(new IntProperty(prop.propID));
                    else 
                        hitboxData.intProps.Remove(hitboxData.intProps.Single(x => x.iD == prop.propID));

                }
                else if(obj.GetType() == typeof(string)){
                    
                    if(!hitboxData.stringProps.Any(x => x.iD == prop.propID))
                        hitboxData.stringProps.Add(new StringProperty(prop.propID));
                    else
                        hitboxData.stringProps.Remove(hitboxData.stringProps.Single(x => x.iD == prop.propID));
                }
                else if(obj.GetType() == typeof(bool)){

                    if(!hitboxData.boolProps.Any(x => x.iD == prop.propID))
                        hitboxData.boolProps.Add(new BoolProperty(prop.propID));
                    else
                        hitboxData.boolProps.Remove(hitboxData.boolProps.Single(x => x.iD == prop.propID));
                }
                else if(obj.GetType() == typeof(float)){

                    if(!hitboxData.floatProps.Any(x => x.iD == prop.propID))
                        hitboxData.floatProps.Add(new FloatProperty(prop.propID));
                    else 
                        hitboxData.floatProps.Remove(hitboxData.floatProps.Single(x => x.iD == prop.propID));

                }
                else if(obj.GetType() == typeof(long)){
                    
                    if(!hitboxData.longProps.Any(x => x.iD == prop.propID))
                        hitboxData.longProps.Add(new LongProperty(prop.propID));
                    else 
                        hitboxData.longProps.Remove(hitboxData.longProps.Single(x => x.iD == prop.propID));
                
                }
                else if(obj.GetType() == typeof(double)){
                    if(!hitboxData.doubleProps.Any(x => x.iD == prop.propID))
                        hitboxData.doubleProps.Add(new DoubleProperty(prop.propID));
                    else 
                        hitboxData.doubleProps.Remove(hitboxData.doubleProps.Single(x => x.iD == prop.propID));
                
                }
                else if(obj.GetType() == typeof(Rect)){
                    if(!hitboxData.rectProps.Any(x => x.iD == prop.propID))
                        hitboxData.rectProps.Add(new RectProperty(prop.propID));
                    else 
                        hitboxData.rectProps.Remove(hitboxData.rectProps.Single(x => x.iD == prop.propID));
                }
                else if(obj.GetType() == typeof(RectInt)){
                    if(!hitboxData.rectIntProps.Any(x => x.iD == prop.propID))
                        hitboxData.rectIntProps.Add(new RectIntProperty(prop.propID));
                    else 
                        hitboxData.rectIntProps.Remove(hitboxData.rectIntProps.Single(x => x.iD == prop.propID));
                }
                else if(obj.GetType() == typeof(Color)){
                
                    if(!hitboxData.colorProps.Any(x => x.iD == prop.propID))
                        hitboxData.colorProps.Add(new ColorProperty(prop.propID));
                    else 
                        hitboxData.colorProps.Remove(hitboxData.colorProps.Single(x => x.iD == prop.propID));
                
                }
                else if(obj.GetType() == typeof(AnimationCurve)){
                    
                    if(!hitboxData.animationCurveProps.Any(x => x.iD == prop.propID))
                        hitboxData.animationCurveProps.Add(new AnimationCurveProperty(prop.propID));
                    else 
                        hitboxData.animationCurveProps.Remove(hitboxData.animationCurveProps.Single(x => x.iD == prop.propID));
                
                }
                else if(obj.GetType() == typeof(Bounds)){
                    if(!hitboxData.boundsProps.Any(x => x.iD == prop.propID))
                        hitboxData.boundsProps.Add(new BoundsProperty(prop.propID));
                    else 
                        hitboxData.boundsProps.Remove(hitboxData.boundsProps.Single(x => x.iD == prop.propID));
                
                }
                else if(obj.GetType() == typeof(BoundsInt)){
                    if(!hitboxData.boundsIntProps.Any(x => x.iD == prop.propID))
                        hitboxData.boundsIntProps.Add(new BoundsIntProperty(prop.propID));
                    else 
                        hitboxData.boundsIntProps.Remove(hitboxData.boundsIntProps.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType() == typeof(Vector2) ){
                    if(!hitboxData.vector2Props.Any(x => x.iD == prop.propID))
                        hitboxData.vector2Props.Add(new Vector2Property(prop.propID));
                    else 
                        hitboxData.vector2Props.Remove(hitboxData.vector2Props.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType() == typeof(Vector3) ){
                    if(!hitboxData.vector3Props.Any(x => x.iD == prop.propID))
                        hitboxData.vector3Props.Add(new Vector3Property(prop.propID));
                    else 
                        hitboxData.vector3Props.Remove(hitboxData.vector3Props.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType() == typeof(Vector4) ){
                    if(!hitboxData.vector4Props.Any(x => x.iD == prop.propID))
                        hitboxData.vector4Props.Add(new Vector4Property(prop.propID));
                    else 
                        hitboxData.vector4Props.Remove(hitboxData.vector4Props.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType() == typeof(Vector2Int)){
                    if(!hitboxData.vector2IntProps.Any(x => x.iD == prop.propID))
                        hitboxData.vector2IntProps.Add(new Vector2IntProperty(prop.propID));
                    else 
                        hitboxData.vector2IntProps.Remove(hitboxData.vector2IntProps.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType() == typeof(Vector3Int)){
                    if(!hitboxData.vector3IntProps.Any(x => x.iD == prop.propID))
                        hitboxData.vector3IntProps.Add(new Vector3IntProperty(prop.propID));
                    else 
                        hitboxData.vector3IntProps.Remove(hitboxData.vector3IntProps.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType().IsSubclassOf(typeof(UnityEngine.Object)) ){
                    if(!hitboxData.unityObjectProps.Any(x => x.iD == prop.propID))
                        hitboxData.unityObjectProps.Add(new UnityObjectProperty(prop.propID));
                    else 
                        hitboxData.unityObjectProps.Remove(hitboxData.unityObjectProps.Single(x => x.iD == prop.propID));
                    
                }
                else if(obj.GetType() == typeof(Gradient) ){
                    if(!hitboxData.gradientProps.Any(x => x.iD == prop.propID))
                        hitboxData.gradientProps.Add(new GradientProperty(prop.propID));
                    else 
                        hitboxData.gradientProps.Remove(hitboxData.gradientProps.Single(x => x.iD == prop.propID));
                    
                }

                else {
                    Debug.Log("Bisey yok alooo");
                }
        }

    }

}

