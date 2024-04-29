using System;
using UnityEngine;

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

    public static float Vector3Dot(Vector3 vec1, Vector3 vec2, bool shouldNormalise = false)
    {
        MyVector3 lhs, rhs;

        lhs = new MyVector3(vec1.x, vec1.y, vec1.z);
        rhs = new MyVector3(vec2.x, vec2.y, vec2.z);
        float dot = 0.0f;

        if (shouldNormalise)
        {
            MyVector3 normalisedVec1 = lhs.NormalizeVector();
            MyVector3 normalisedVec2 = rhs.NormalizeVector();

            dot = normalisedVec1.x * normalisedVec2.x + normalisedVec1.y * normalisedVec2.y + normalisedVec1.z * normalisedVec2.z;
        }
        else
        {
            dot = lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
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

    public static Vector3 RotateVector(Vector3 vector, float angle, Vector3 axis)
    {
       return vector * Mathf.Cos(angle) + ((MathLib.VectorCrossProduct(axis, vector) * Mathf.Sin(angle)) + axis * (MyVector3.Vector3Dot(axis, vector) * (1 - Mathf.Cos(angle))));
    }

}
