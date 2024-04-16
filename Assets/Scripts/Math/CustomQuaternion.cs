using System;
using UnityEngine;

public class CustomQuaternion
{
    public float x, y, z, w;

    public CustomQuaternion(float angle, Vector3 axis)
    {
        float halfangle = angle / 2;
        w = Mathf.Cos(halfangle);
        x = axis.x * Mathf.Sin(halfangle);
        y = axis.y * Mathf.Sin(halfangle);
        z = axis.z * Mathf.Sin(halfangle);
    }

    public CustomQuaternion(Vector3 vertex)
    {
        x = vertex.x;
        y = vertex.y;
        z = vertex.z;
    }

    public CustomQuaternion()
    {

    }

    public static CustomQuaternion operator *(CustomQuaternion lhs, CustomQuaternion rhs)
    {
        Vector3 rhsvec = new Vector3(rhs.x, rhs.y, rhs.z);
        Vector3 lhsvec = new Vector3(lhs.x, lhs.y, lhs.z);

        CustomQuaternion rv = new CustomQuaternion( rhs.w * lhsvec + lhs.w * rhsvec + MathLib.VectorCrossProduct(lhsvec, rhsvec));
        rv.w = rhs.w * lhs.w - MyVector3.Vector3Dot(rhsvec, lhsvec);

        return rv;
    }

    public static CustomQuaternion operator /(CustomQuaternion lhs, float rhs)
    {
        Vector3 axis = new Vector3(lhs.x / rhs, lhs.y / rhs, lhs.z /rhs);

        float newW = lhs.w / rhs;

        CustomQuaternion rv = new CustomQuaternion(newW, axis);

        return rv;
    }

    public CustomQuaternion Inverse()
    {
        CustomQuaternion rv = new CustomQuaternion();

        rv.w = w;

        rv.SetAxis(-GetAxis());

        return rv;


    }

    public float Magnitude()
    {
        return Mathf.Sqrt((w * w) + (x*x) + (y*y) + (z*z));
    }

    private void SetAxis(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 GetAxis()
    {
        return new Vector3(x, y, z);
    }

    public Quaternion ConvertToUnityQuaternion()
    {
        return new Quaternion(x,y,z,w);
    }

    public Vector4 GetAxisAngle()
    {
        Vector4 rv = new Vector4();

        float halfangle = Mathf.Acos(w);
        rv.w = halfangle * 2;

        float sinValue = Mathf.Sin(halfangle);

        if(sinValue == 0)
        {
            rv.x = 0;
            rv.y = 0;
            rv.z = 0;

            return rv;
        }

        rv.x = x / Mathf.Sin(halfangle);
        rv.y = y / Mathf.Sin(halfangle);
        rv.z = z / Mathf.Sin(halfangle);


        return rv;
    }
    public static CustomQuaternion Slerp(CustomQuaternion a, CustomQuaternion b,float t)
    {
        t = Mathf.Clamp01(t);

        CustomQuaternion d = b * a.Inverse();
        Vector4 AxisAngle = d.GetAxisAngle();
        CustomQuaternion dT = new(AxisAngle.w * t, new Vector3(AxisAngle.x, AxisAngle.y, AxisAngle.z));

        return dT * a;
    }

    public Matrix4by4 ConvertToMatrix()
    {
        Matrix4by4 rv = new Matrix4by4(
            new Vector3(2 * (w * w + x * x) - 1, 2 * (x * y - w * z), 2 * (x * z + w * y)),
            new Vector3(2 * (x * y + w * z), 2 * (w * w + y * y) - 1, 2 * (y * z - w * x)),
            new Vector3(2 * (x * z - w * y), 2 * (y * z + w * x), 2 * (w * w + z * z) - 1),
            Vector3.zero
            );
        return rv;
    }
}
