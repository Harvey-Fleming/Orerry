using System;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class MyVector3
{
    public float x, y, z;

    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyVector3(Vector3 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }

    public static MyVector3 AddVectors(MyVector3 vec1, MyVector3 vec2)
    {
        MyVector3 tempMyVector3 = new MyVector3(0,0,0);

        tempMyVector3.x = vec1.x + vec2.x;
        tempMyVector3.y = vec1.x + vec2.y;
        tempMyVector3.z = vec1.x + vec2.z;

        return tempMyVector3;
    }    
    
    public static MyVector3 SubtractVectors(MyVector3 vec1, MyVector3 vec2)
    {
        MyVector3 tempMyVector3 = new MyVector3(0,0,0);

        tempMyVector3.x = vec1.x - vec2.x;
        tempMyVector3.y = vec1.y - vec2.y;
        tempMyVector3.z = vec1.z - vec2.z;

        return tempMyVector3;
    }

    public static MyVector3 ScaleVector(MyVector3 vec1, float scalar)
    {
        return new MyVector3(vec1.x * scalar, vec1.y * scalar, vec1.z * scalar);
    }    

    public static MyVector3 operator *(MyVector3 vec1, MyVector3 vec2)
    {
        return new MyVector3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
    }

    public static MyVector3 DivideVector(MyVector3 vec1, float scalar)
    {
        return new MyVector3(vec1.x / scalar, vec1.y / scalar, vec1.z / scalar);
    }

    public static float VectorDot(MyVector3 vec1, MyVector3 vec2, bool shouldNormalise = false)
    {
        float dot = 0.0f;

        if (shouldNormalise)
        {
            MyVector3 normalisedVec1 = vec1.NormalizeVector();
            MyVector3 normalisedVec2 = vec2.NormalizeVector();

            dot = normalisedVec1.x * normalisedVec2.x + normalisedVec1.y * normalisedVec2.y + normalisedVec1.z * normalisedVec2.z;
        }
        else
        {
            dot = vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
        }


        return dot;
    }

    public float GetLength()
    {
        float length = 0.0f;

        length = Mathf.Sqrt(x * x + y * y + z * z);

        return length;
    }    
    
    public float GetLengthSqr()
    {
        float length = 0.0f;

        length = x * x + y * y + z * z;

        return length;
    }    
    
    public MyVector3 NormalizeVector()
    {
        MyVector3 tempMyVector3 = new MyVector3(0, 0, 0);

        tempMyVector3 = DivideVector(this, GetLength());

        return tempMyVector3;
    }

    public Vector3 ConvertToUnityVector()
    {
        return new Vector3(x, y, z);
    }

    // Vector3.MoveTowards
    public static Vector3 MoveTowards(MyVector3 current, MyVector3 target, float maxDistanceDelta)
    {
        MyVector3 a = SubtractVectors(target, current);
        float magnitude = a.GetLength();
        if (magnitude <= maxDistanceDelta || magnitude == 0f)
        {
            return target.ConvertToUnityVector();
        }
        return current.ConvertToUnityVector() + a.ConvertToUnityVector() / magnitude * maxDistanceDelta;
    }

}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(MyVector3))]
public class VectorInspectorPropertyDrawer : PropertyDrawer
{

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();
        
        // Create property fields.
        var x = new PropertyField(property.FindPropertyRelative("x"));
        var y = new PropertyField(property.FindPropertyRelative("y"));
        var z = new PropertyField(property.FindPropertyRelative("z"));

        // Add fields to the container.
        container.Add(x);
        container.Add(y);
        container.Add(z);

        return container;
    }

}

[CustomEditor(typeof(VectorInspector))]
public class VectorInspectorEditor : Editor
{
    VectorInspector myTarget;

    public void OnEnable()
    {
        myTarget = (VectorInspector)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 10f;
        EditorGUILayout.BeginHorizontal();
        myTarget.Position.x = EditorGUILayout.FloatField("X", myTarget.Position.x);
        myTarget.Position.y = EditorGUILayout.FloatField("Y", myTarget.Position.y);
        myTarget.Position.z = EditorGUILayout.FloatField("Z", myTarget.Position.z);
        EditorGUILayout.EndHorizontal();
    }

}


#endif
