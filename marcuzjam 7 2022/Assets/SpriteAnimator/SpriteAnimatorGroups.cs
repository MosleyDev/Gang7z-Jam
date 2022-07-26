using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PixelAnimator;

namespace PixelAnimator{


    [System.Serializable]
    public class Group{
        public Color color = Color.black;
        public string boxType;
        public int activeLayer;
        public PhysicsMaterial2D physicMaterial;
        public bool rounded;
        

        [Space(10)]
        public int groupID;
        

    }
    [System.Serializable]
    public class Layer{
        public Group group;
        public List<Frame> frames;
        public bool reSizeBox;
        public bool onHandleTopleft;
        public bool onHandleTopCenter;
        public bool onHandleTopRight;
        public bool onHandleRightCenter;
        public bool onHandleBottomRight;
        public bool onHandleBottomCenter;
        public bool onHandleBottomLeft;
        public bool onHandleLeftCenter;
        


        public Layer(){
            group = new Group();
            frames = new List<Frame>();
            
        
        }

    }


    [System.Serializable]
    public class Frame{

        public enum ColissionTypes{NoTrigger, Trigger}
        public HitboxData hitboxData;
        public ColissionTypes colissionTypes;
        
        public Rect hitboxRect;

        public Frame(){
            hitboxData = new HitboxData();
            hitboxRect = new Rect(16, 16, 16, 16);
        }


    }


    public enum GetComponentForWay{Parent, ObjectUsed, Manuel}


    [System.Serializable]

    public class Property{
        
        public bool isDataActive = true;
        public int iD;

        public Property(int iD){
            this.iD = iD;
        }

    }
    [System.Serializable]
    public class HitboxData{
        public List<IntProperty> intProps;
        public List<StringProperty> stringProps;
        public List<BoolProperty> boolProps;
        public List<FloatProperty> floatProps;
        public List<DoubleProperty> doubleProps;
        public List<LongProperty> longProps;
        public List<RectProperty> rectProps;
        public List<RectIntProperty> rectIntProps;
        public List<ColorProperty> colorProps;
        public List<AnimationCurveProperty> animationCurveProps;
        public List<BoundsProperty> boundsProps;
        public List<BoundsIntProperty> boundsIntProps;
        public List<Vector2Property> vector2Props;
        public List<Vector3Property> vector3Props;
        public List<Vector4Property> vector4Props;
        public List<Vector2IntProperty> vector2IntProps;
        public List<Vector3IntProperty> vector3IntProps;
        public List<UnityObjectProperty> unityObjectProps;
        public List<GradientProperty> gradientProps;

        public HitboxData()
        {
            this.intProps = new List<IntProperty>();
            this.stringProps = new List<StringProperty>();
            this.boolProps = new List<BoolProperty>();
            this.floatProps = new List<FloatProperty>();
            this.doubleProps = new List<DoubleProperty>();
            this.longProps = new List<LongProperty>();
            this.rectProps = new List<RectProperty>();
            this.rectIntProps = new List<RectIntProperty>();
            this.colorProps = new List<ColorProperty>();
            this.animationCurveProps = new List<AnimationCurveProperty>();
            this.boundsProps = new List<BoundsProperty>();
            this.boundsIntProps = new List<BoundsIntProperty>();
            this.vector2Props = new List<Vector2Property>();
            this.vector3Props = new List<Vector3Property>();
            this.vector4Props = new List<Vector4Property>();
            this.vector2IntProps = new List<Vector2IntProperty>();
            this.vector3IntProps = new List<Vector3IntProperty>();
            this.unityObjectProps = new List<UnityObjectProperty>();
            this.gradientProps = new List<GradientProperty>();
        }
    }




    [System.Serializable]
    public class IntProperty : Property{
        public int data;

        public IntProperty(int iD) : base(iD){

        }
        
    }
    [System.Serializable]
    public class StringProperty : Property{
        public string data;

        public StringProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class BoolProperty : Property{
        public bool data;

        public BoolProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class FloatProperty : Property{
        public float data;

        public FloatProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class DoubleProperty : Property{
        public double data;

        public DoubleProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class LongProperty : Property{
        public long data;

        public LongProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class RectProperty : Property{
        public Rect data;

        public RectProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class RectIntProperty : Property{
        public RectInt data;

        public RectIntProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class ColorProperty : Property{
        public Color data;

        public ColorProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class AnimationCurveProperty : Property{
        public AnimationCurve data;

        public AnimationCurveProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class BoundsProperty : Property{
        public Bounds data;

        public BoundsProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class BoundsIntProperty : Property{
        public BoundsInt data;

        public BoundsIntProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class Vector2Property : Property{
        public Vector2 data;

        public Vector2Property(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class Vector3Property : Property{
        public Vector3 data;

        public Vector3Property(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class Vector4Property : Property{
        public Vector4 data;

        public Vector4Property(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class Vector2IntProperty : Property{
        public Vector2Int data;

        public Vector2IntProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class Vector3IntProperty : Property{
        public Vector3Int data;

        public Vector3IntProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class UnityObjectProperty : Property{
        public UnityEngine.Object data;

        public UnityObjectProperty(int iD) : base(iD)
        {
        }
    }
    [System.Serializable]
    public class GradientProperty : Property{
        public Gradient data;

        public GradientProperty(int iD) : base(iD)
        {
        }
    }








    [System.Serializable]
    public class SpriteProperty {

        public string name;
        
        public SerializableSystemType componentType;
        public SerializableSystemType dataType;
        public string dataName;
        
        // [HideInInspector]
        public GenericMenu selectedData;
        [HideInInspector]
        public GetComponentForWay componentWay;
        public int propID;    

        
    }

    [System.Serializable]
    public class HitboxProperty{
        public string name;
        public SerializableSystemType componentType;
        public SerializableSystemType dataType;
        public string dataName;
        [HideInInspector]
        public GetComponentForWay componentWay;
        public int propID;
    }

        



}
