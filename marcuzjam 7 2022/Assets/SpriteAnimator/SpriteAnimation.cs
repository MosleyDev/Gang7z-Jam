using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


namespace PixelAnimator{

    [CreateAssetMenu(menuName = "SpriteAnimation/ SpriteSheetAnimation")]
    public class SpriteAnimation : ScriptableObject {

        public bool loop = true;
        public float frameRate;
        public List<Sprite> sprites;
        // public List<SpriteFrameProperties> frameProperties;
        // public List<Frame> frames;
        public List<Layer> layers;


        public void AddLayer(Group addedGroup){
            layers.Add(new Layer());
            int index = layers.Count -1;

            var frames = layers[index].frames;
            if(frames == null) 
                frames = new List<Frame>();

            for(int f = 0; f < sprites.Count; f++){
                frames.Add(new Frame());
            }

            layers[index].group = addedGroup;

        }

        


        public T GetPropertyValue<T>(int frameIndex, string boxType, int dataID){

            var type = typeof(T);
            var layer = layers.FirstOrDefault(x => x.group.boxType == boxType);
            var hitboxData = layer.frames[frameIndex].hitboxData;

            if(type == typeof(int)){
                var intProp = hitboxData.intProps.FirstOrDefault(x => x.iD == dataID);
                if(intProp != null)
                    return (T)Convert.ChangeType(intProp.data, typeof(T));
                
                return (T)Convert.ChangeType(0, typeof(T));
            }
            else if(type == typeof(string)){
                var stringProp = hitboxData.stringProps.FirstOrDefault(x => x.iD == dataID);
                return (T)Convert.ChangeType(stringProp.data, typeof(T));
            }
            else if(type == typeof(bool)){
                var boolProp = hitboxData.boolProps.FirstOrDefault(x => x.iD == dataID);
                return (T)Convert.ChangeType(boolProp.data, typeof(T));
            }
            else if(type == typeof(float)){
                var floatProp = hitboxData.floatProps.FirstOrDefault(x => x.iD == dataID);
                if(floatProp != null)
                    return (T)Convert.ChangeType(floatProp.data, typeof(T));
                return (T)Convert.ChangeType(0, typeof(T));
            }
            else if(type == typeof(long)){
                var longProp = hitboxData.longProps.FirstOrDefault(x => x.iD == dataID);
                if(longProp != null)
                    return (T)Convert.ChangeType(longProp.data, typeof(T));
                return (T)Convert.ChangeType(0, typeof(T));
            }
            else if(type == typeof(double)){
                var doubleProps = hitboxData.doubleProps.FirstOrDefault(x => x.iD == dataID);
                if(doubleProps != null)
                    return (T)Convert.ChangeType(doubleProps.data, typeof(T));
                return (T)Convert.ChangeType(0, typeof(T));
            }
            else if(type == typeof(Rect)){
                var rectProp = hitboxData.rectProps.FirstOrDefault(x => x.iD == dataID);
                if(rectProp != null)
                    return (T)Convert.ChangeType(rectProp.data, typeof(T));
                return (T)Convert.ChangeType(new Rect(0,0,0,0), typeof(T));
            }
            else if(type == typeof(RectInt)){
                var rectIntProp = hitboxData.rectIntProps.FirstOrDefault(x => x.iD == dataID);
                if(rectIntProp != null)
                    return (T)Convert.ChangeType(rectIntProp.data, typeof(T));
                return (T)Convert.ChangeType(new RectInt(0,0,0,0), typeof(T));
                
            }
            else if(type == typeof(Color)){
                var colorProp = hitboxData.colorProps.FirstOrDefault(x => x.iD == dataID);
                if(colorProp != null)
                    return (T)Convert.ChangeType(colorProp.data, typeof(T));    
                return (T)Convert.ChangeType(colorProp.data, typeof(T));
                
            }
            else if(type == typeof(AnimationCurve)){
                var animationCurveProp = hitboxData.animationCurveProps.FirstOrDefault(x => x.iD == dataID);
                return (T)Convert.ChangeType(animationCurveProp.data, typeof(T));
            }
            else if(type == typeof(Bounds)){
                var boundsProp = hitboxData.boundsProps.FirstOrDefault(x => x.iD == dataID);
                if(boundsProp != null)
                    return (T)Convert.ChangeType(boundsProp.data, typeof(T));
                return (T)Convert.ChangeType(new Bounds(), typeof(T));
            }
            else if(type == typeof(BoundsInt)){
                var boundsIntProp = hitboxData.boundsIntProps.FirstOrDefault(x => x.iD == dataID);
                if(boundsIntProp != null)
                    return (T)Convert.ChangeType(boundsIntProp.data, typeof(T));
                return (T)Convert.ChangeType(new BoundsInt(), typeof(T));
            }
            else if(type == typeof(Vector2)){
                var vector2Prop = hitboxData.vector2Props.FirstOrDefault(x => x.iD == dataID);
                if(vector2Prop != null)
                    return (T)Convert.ChangeType(vector2Prop.data, typeof(T));
                return default(T);
            }
            else if(type == typeof(Vector3)){
                var vector3Prop = hitboxData.vector3Props.FirstOrDefault(x => x.iD == dataID);
                if(vector3Prop != null)
                    return (T)Convert.ChangeType(vector3Prop.data, typeof(T));
                return (T)Convert.ChangeType(new Vector3(0,0), typeof(T));
            }
            else if(type == typeof(Vector4)){
                var vector4Prop = hitboxData.vector4Props.FirstOrDefault(x => x.iD == dataID);
                if(vector4Prop != null)
                    return (T)Convert.ChangeType(vector4Prop.data, typeof(T));
                return (T)Convert.ChangeType(new Vector4(0,0), typeof(T));
            }
            else if(type == typeof(Vector2Int)){
                var vector2IntProp = hitboxData.vector2IntProps.FirstOrDefault(x => x.iD == dataID);
                if(vector2IntProp != null)
                    return (T)Convert.ChangeType(vector2IntProp.data, typeof(T));
                return (T)Convert.ChangeType(new Vector2Int(0,0), typeof(T));
            }
            else if(type == typeof(Vector3Int)){
                var vector3IntProp = hitboxData.vector3IntProps.FirstOrDefault(x => x.iD == dataID);
                if(vector3IntProp != null)
                    return (T)Convert.ChangeType(vector3IntProp.data, typeof(T));
                return (T)Convert.ChangeType(new Vector3Int(0,0,0), typeof(T));
            }
            else if(type == typeof(Component)){
                var componentProp = hitboxData.unityObjectProps.FirstOrDefault(x => x.iD == dataID);
                return (T)Convert.ChangeType(componentProp.data, typeof(T));
            }
            else if(type == typeof(Gradient)){
                var gradientProp = hitboxData.gradientProps.FirstOrDefault(x => x.iD == dataID);
                return (T)Convert.ChangeType(gradientProp.data, typeof(T));
            }
            else{
            return default(T);
            }



            
        }
        public List<PropFrame<T>> GetListOfProperties<T>(string boxType, int dataID){

            var type = typeof(T);
            var layer = layers.FirstOrDefault(x => x.group.boxType == boxType);
            
            List<PropFrame<T>> propValue = new List<PropFrame<T>>();

            for(int f = 0; f < layer.frames.Count; f++){
            
                var hitboxData = layer.frames[f].hitboxData;   
                
                if(type == typeof(int)){
                    var intProp = hitboxData.intProps.FirstOrDefault(x => x.iD == dataID);
                    if(intProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(intProp.data, typeof(T)), f));

                }
                else if(type == typeof(string)){
                    var stringProp = hitboxData.stringProps.FirstOrDefault(x => x.iD == dataID);
                    if(stringProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(stringProp.data, typeof(T)), f));

                }
                else if(type == typeof(bool)){
                    var boolProp = hitboxData.boolProps.FirstOrDefault(x => x.iD == dataID);
                    if(boolProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(boolProp.data, typeof(T)), f));
                }
                else if(type == typeof(float)){
                    var floatProp = hitboxData.floatProps.FirstOrDefault(x => x.iD == dataID);
                    if(floatProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(floatProp.data, typeof(T)), f));

                }
                else if(type == typeof(long)){
                    var longProp = hitboxData.longProps.FirstOrDefault(x => x.iD == dataID);
                    if(longProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(longProp.data, typeof(T)), f));

                }
                else if(type == typeof(double)){
                    var doubleProps = hitboxData.doubleProps.FirstOrDefault(x => x.iD == dataID);
                    if(doubleProps != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(doubleProps.data, typeof(T)), f));

                }
                else if(type == typeof(Rect)){
                    var rectProp = hitboxData.rectProps.FirstOrDefault(x => x.iD == dataID);
                    if(rectProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(rectProp.data, typeof(T)), f));

                }
                else if(type == typeof(RectInt)){
                    var rectIntProp = hitboxData.rectIntProps.FirstOrDefault(x => x.iD == dataID);
                    if(rectIntProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(rectIntProp.data, typeof(T)), f));

                    
                }
                else if(type == typeof(Color)){
                    var colorProp = hitboxData.colorProps.FirstOrDefault(x => x.iD == dataID);
                    if(colorProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(colorProp.data, typeof(T)), f));

                }
                else if(type == typeof(AnimationCurve)){
                    var animationCurveProp = hitboxData.animationCurveProps.FirstOrDefault(x => x.iD == dataID);
                    if(animationCurveProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(animationCurveProp.data, typeof(T)), f));
                }
                else if(type == typeof(Bounds)){
                    var boundsProp = hitboxData.boundsProps.FirstOrDefault(x => x.iD == dataID);
                    if(boundsProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(boundsProp.data, typeof(T)), f));

                }
                else if(type == typeof(BoundsInt)){
                    var boundsIntProp = hitboxData.boundsIntProps.FirstOrDefault(x => x.iD == dataID);
                    if(boundsIntProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(boundsIntProp.data, typeof(T)), f));

                }
                else if(type == typeof(Vector2)){
                    var vector2Prop = hitboxData.vector2Props.FirstOrDefault(x => x.iD == dataID);
                    if(vector2Prop != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(vector2Prop.data, typeof(T)), f));

                }
                else if(type == typeof(Vector3)){
                    var vector3Prop = hitboxData.vector3Props.FirstOrDefault(x => x.iD == dataID);
                    if(vector3Prop != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(vector3Prop.data, typeof(T)), f));

                }
                else if(type == typeof(Vector4)){
                    var vector4Prop = hitboxData.vector4Props.FirstOrDefault(x => x.iD == dataID);
                    if(vector4Prop != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(vector4Prop.data, typeof(T)), f));

                }
                else if(type == typeof(Vector2Int)){
                    var vector2IntProp = hitboxData.vector2IntProps.FirstOrDefault(x => x.iD == dataID);
                    if(vector2IntProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(vector2IntProp.data, typeof(T)), f));

                }
                else if(type == typeof(Vector3Int)){
                    var vector3IntProp = hitboxData.vector3IntProps.FirstOrDefault(x => x.iD == dataID);
                    if(vector3IntProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(vector3IntProp.data, typeof(T)), f));

                }
                else if(type == typeof(Component)){
                    var componentProp = hitboxData.unityObjectProps.FirstOrDefault(x => x.iD == dataID);
                    if(componentProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(componentProp.data, typeof(T)), f));

                }
                else if(type == typeof(Gradient)){
                    var gradientProp = hitboxData.gradientProps.FirstOrDefault(x => x.iD == dataID);
                    if(gradientProp != null)
                        propValue.Add(new PropFrame<T>((T)Convert.ChangeType(gradientProp.data, typeof(T)), f));

                }
            }
            return propValue;
            
        }

        
    }

    [Serializable]
    public class PropFrame<T>{

        public T data;
        public int frameIndex;

        public PropFrame(T data, int frameIndex){
            this.data = data;
            this.frameIndex = frameIndex;
        }
    }

}



