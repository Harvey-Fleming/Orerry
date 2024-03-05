using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix4by4
{
    public Matrix4by4(Vector4 column1, Vector4 column2, Vector4 column3, Vector4 column4)
    {
        values = new float[4, 4];

        //Column 1 Constructor
        values[0, 0] = column1.x;
        values[1, 0] = column1.y;
        values[2, 0] = column1.z;
        values[3, 0] = column1.w;        
        
        //Column 2 Constructor
        values[0, 1] = column2.x;
        values[1, 1] = column2.y;
        values[2, 1] = column2.z;
        values[3, 1] = column2.w;        
        
        //Column 3 Constructor
        values[0, 2] = column3.x;
        values[1, 2] = column3.y;
        values[2, 2] = column3.z;
        values[3, 2] = column3.w;        
        
        //Column 4 Constructor
        values[0, 3] = column4.x;
        values[1, 3] = column4.y;
        values[2, 3] = column4.z;
        values[3, 3] = column4.w;
    }

    public Matrix4by4(Vector3 column1, Vector3 column2, Vector3 column3, Vector3 column4)
    {
        values = new float[4, 4];

        //Column 1 Constructor
        values[0, 0] = column1.x;
        values[1, 0] = column1.y;
        values[2, 0] = column1.z;
        values[3, 0] = 0;

        //Column 2 Constructor
        values[0, 1] = column2.x;
        values[1, 1] = column2.y;
        values[2, 1] = column2.z;
        values[3, 1] = 0;

        //Column 3 Constructor
        values[0, 2] = column3.x;
        values[1, 2] = column3.y;
        values[2, 2] = column3.z;
        values[3, 2] = 0;

        //Column 4 Constructor
        values[0, 3] = column4.x;
        values[1, 3] = column4.y;
        values[2, 3] = column4.z;
        values[3, 3] = 1;
    }

    private float[,] values;

    public static Matrix4by4 Identity
    { 
        get
        {
            return new Matrix4by4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1));
        }
    }

    public Vector4 GetRow(int rowNum)
    {
        return new Vector4(values[rowNum, 0], values[rowNum, 1], values[rowNum, 2], values[rowNum, 3]);
    }

    public static Vector4 operator *(Matrix4by4 lhs, Vector4 vector)
    {
        float newX = lhs.values[0, 0] * vector.x + lhs.values[0, 1] * vector.y + lhs.values[0, 2] * vector.z + lhs.values[0, 3] * vector.w;
        float newY = lhs.values[1, 0] * vector.x + lhs.values[1, 1] * vector.y + lhs.values[1, 2] * vector.z + lhs.values[1, 3] * vector.w;
        float newZ = lhs.values[2, 0] * vector.x + lhs.values[2, 1] * vector.y + lhs.values[2, 2] * vector.z + lhs.values[2, 3] * vector.w;
        float newW = lhs.values[3, 0] * vector.x + lhs.values[3, 1] * vector.y + lhs.values[3, 2] * vector.z + lhs.values[3, 3] * vector.w;

        return new Vector4(newX, newY, newZ, newW);
    }    
    
    public static Matrix4by4 operator *(Matrix4by4 lhs, Matrix4by4 rhs)
    {
        Matrix4by4 rv = Identity;

        for(int i = 0; i < 4; i++)
        {
            rv.values[0, i] = lhs.values[0, 0] * rhs.values[0, i] + lhs.values[0, 1] * rhs.values[1, i] + lhs.values[0, 2] * rhs.values[2, i] + lhs.values[0, 3] * rhs.values[3, i];
            rv.values[1, i] = lhs.values[1, 0] * rhs.values[0, i] + lhs.values[1, 1] * rhs.values[1, i] + lhs.values[1, 2] * rhs.values[2, i] + lhs.values[1, 3] * rhs.values[3, i];
            rv.values[2, i] = lhs.values[2, 0] * rhs.values[0, i] + lhs.values[2, 1] * rhs.values[1, i] + lhs.values[2, 2] * rhs.values[2, i] + lhs.values[2, 3] * rhs.values[3, i];
            rv.values[3, i] = lhs.values[3, 0] * rhs.values[0, i] + lhs.values[3, 1] * rhs.values[1, i] + lhs.values[3, 2] * rhs.values[2, i] + lhs.values[3, 3] * rhs.values[3, i];
        }

        return rv;
    }

    public Matrix4by4 TranslationInverse()
    {
        Matrix4by4 rv = Identity;

        rv.values[0, 3] = -values[0, 3];
        rv.values[1, 3] = -values[1, 3];
        rv.values[2, 3] = -values[2, 3];

        return rv;
    }

    public Matrix4by4 ScaleInverse()
    {
        Matrix4by4 rv = Identity;

        rv.values[0, 0] = 1.0f / values[0, 0];
        rv.values[1, 1] = 1.0f / values[1, 1];
        rv.values[2, 2] = 1.0f / values[2, 2];

        return rv;
    }

    public Matrix4by4 RotationInverse()
    {
        return new Matrix4by4(GetRow(0), GetRow(1), GetRow(2), GetRow(3));
    }

}
