using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] GameObject target;

    // Update is called once per frame
    void Update()
    {
        CustomTransform targetTransform = target.GetComponent<CustomTransform>();

        Vector3 dir = MyVector3.SubtractVectors(new MyVector3(targetTransform.Position), new MyVector3(transform.position)).ConvertToUnityVector();

        //Calculate forward direction
        Vector3 forwardDirection = dir;
        //Debug.Log(forwardDirection);
        Debug.DrawRay(transform.position, forwardDirection, Color.red, 0.1f);
        //Calculate right direction
        Vector3 rightDirection = MathLib.VectorCrossProduct(Vector3.up, forwardDirection);
        Debug.DrawRay(transform.position, rightDirection, Color.blue, 0.1f);

        Vector3 updirection = MathLib.VectorCrossProduct(forwardDirection, rightDirection);
        Debug.DrawRay(transform.position, updirection, Color.yellow, 0.1f);

        Debug.Log(forwardDirection.magnitude);
        Debug.Log(rightDirection.magnitude);
        Debug.Log(updirection.magnitude);
        float pitchangle =  Mathf.Atan2(updirection.normalized.magnitude, forwardDirection.magnitude) * 180/Mathf.PI;
        float yawangle = Mathf.Atan2(rightDirection.magnitude, forwardDirection.magnitude) * 180/Mathf.PI;

        transform.rotation = Quaternion.Euler(pitchangle, 180, 0);
    }
}
