using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB
{
    private Vector3 MinExtent;
    private Vector3 MaxExtent;

    public AABB(Vector3 Min, Vector3 Max)
    {
        MinExtent = Min;
        MaxExtent = Max;
    }

    public float Top
    { 
        get
        {
            return MaxExtent.y;
        }
    }    

    public float Bottom
    { 
        get
        {
            return MinExtent.y;
        }
    }    
    
    public float Left
    { 
        get
        {
            return MinExtent.x;
        }
    }

    public float Right
    {
        get
        {
            return MaxExtent.x;
        }
    }

    public float Front
    {
        get
        {
            return MaxExtent.z;
        }
    }

    public float Back
    {
        get
        {
            return MinExtent.z;
        }
    }

    public static bool Intersects(AABB Box1, AABB Box2)
    {
        return !(Box2.Left > Box1.Right 
            || Box2.Right < Box1.Left
            || Box2.Top < Box1.Bottom
            || Box2.Bottom > Box1.Top
            || Box2.Back > Box1.Front
            || Box2.Front < Box1.Back);
    }

    public static bool LineIntersection(AABB Box, Vector3 StartPoint, Vector3 EndPoint, out Vector3 IntersectionPoint)
    {
        float Lowest = 0.0f;
        float Highest = 1.0f;

        IntersectionPoint = Vector3.zero;

        if (!IntersectingAxis(Vector3.right, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
            return false;
        if (!IntersectingAxis(Vector3.up, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
            return false;
        if (!IntersectingAxis(Vector3.forward, Box, StartPoint, EndPoint, ref Lowest, ref Highest))
            return false;

        IntersectionPoint = MathLib.Vec3Lerp(StartPoint, EndPoint, Lowest);

        return true;
    }
    
    public static bool IntersectingAxis(Vector3 Axis, AABB Box, Vector3 StartPoint, Vector3 EndPoint, ref float lowest, ref float Highest)
    {
        float Minimum = 0.0f, Maximum = 1.0f;

        if(Axis == Vector3.right)
        {
            Minimum = (Box.Left - StartPoint.x) / (EndPoint.x - StartPoint.x);
            Maximum = (Box.Right - StartPoint.x) / (EndPoint.x - StartPoint.x);
        }
        else if(Axis == Vector3.up)
        {
            Minimum = (Box.Bottom - StartPoint.y) / (EndPoint.y - StartPoint.y);
            Maximum = (Box.Top - StartPoint.y) / (EndPoint.y - StartPoint.y);
        }
        else if(Axis == Vector3.forward)
        {
            Minimum = (Box.Back - StartPoint.z) / (EndPoint.z - StartPoint.z);
            Maximum = (Box.Front - StartPoint.z) / (EndPoint.z - StartPoint.z);
        }

        if(Maximum < Minimum)
        {
            float temp = Maximum;
            Maximum = Minimum;
            Minimum = temp;
        }

        if (Maximum < lowest)
            return false;

        if (Minimum > Highest)
            return false;

        lowest = Mathf.Max(Minimum, lowest);
        Highest = Mathf.Min(Maximum, Highest);

        if (lowest > Highest)
            return false;

        return true;

    }
}
