using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathLib : MonoBehaviour
{
    public static float VectorToRadians(Vector2 V)
    {
        float radians = 0.0f;

        radians = Mathf.Atan(V.y / V.x);

        return radians;
    }    
    
    public static Vector2 RadiansToVector(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }    
    
    public static Vector3 EulerAnglestoDirection(Vector3 EulerAngles)
    {
        return new Vector3(Mathf.Cos(EulerAngles.x) * Mathf.Sin(EulerAngles.y), Mathf.Sin(EulerAngles.x), Mathf.Cos(EulerAngles.x) * Mathf.Cos(EulerAngles.y));
    }    
    
    public static Vector3 VectorCrossProduct(Vector3 vec1, Vector3 vec2)
    {
        Vector3 resultVec = new Vector3();

        resultVec.x = vec1.y * vec2.z - vec1.z * vec2.y;
        resultVec.y = vec1.z * vec2.x - vec1.x * vec2.z;
        resultVec.z = vec1.x * vec2.y - vec1.y * vec2.x;
        
        return resultVec;
    }

    public static Vector3 Vec3Lerp(Vector3 vector1, Vector3 vector2, float t)
    {
        return vector1 * (1.0f - t) + vector2 * t;
    }

    public static float FloatLerp(float current, float target, float t)
    {
        return current * (1.0f - t) + target * t;
    }


}
