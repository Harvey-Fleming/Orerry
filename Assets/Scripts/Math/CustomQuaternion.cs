using System;
using System.Collections;
using System.Collections.Generic;
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

    public CustomQuaternion Inverse()
    {
        CustomQuaternion rv = new CustomQuaternion();

        rv.w = w;

        rv.SetAxis(-GetAxis());

        return rv;


    }

    private void SetAxis(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 GetAxis()
    {
        return new Vector3(x,y,z);
    }

    public static void RotateAround(CustomTransform transform, Vector3 pivotPoint, Vector3 axis, float angle)
    {
        CustomQuaternion rot = new CustomQuaternion(angle, axis);

        CustomQuaternion p = new CustomQuaternion(transform.Position);
        CustomQuaternion q = new CustomQuaternion(pivotPoint);


        CustomQuaternion newK = q * p * q.Inverse();
        CustomQuaternion newP =  newK * rot;
        transform.Position = new Vector3(newP.x, newP.y, newP.z);
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
}
