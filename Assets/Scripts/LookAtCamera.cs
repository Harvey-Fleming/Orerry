using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] GameObject target;

    Camera mainCamera;

    bool isZoomed = false;
    bool isZooming = false;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtTarget();

        if(Input.GetMouseButtonDown(0))
        {
            isZoomed = !isZoomed;
            isZooming = true;
        }

        if(isZooming)
        {
            if(isZoomed)
            {
                float nextFOV = MathLib.FloatLerp(mainCamera.fieldOfView, 60, Time.deltaTime);
                mainCamera.fieldOfView = nextFOV;
            }
            else
            {
                float nextFOV = MathLib.FloatLerp(mainCamera.fieldOfView, 20, Time.deltaTime);
                mainCamera.fieldOfView = nextFOV;
            }
        }
    }

    private void LookAtTarget()
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

        float pitchangle = 180 - Mathf.Atan2(-forwardDirection.y, forwardDirection.z) * 180 / Mathf.PI;
        float yawangle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * 180 / Mathf.PI;

        float nextPitch = MathLib.FloatLerp(transform.eulerAngles.x, pitchangle, Time.deltaTime);
        float nextYaw = MathLib.FloatLerp(transform.eulerAngles.y, yawangle, Time.deltaTime);

        transform.rotation = Quaternion.Euler(nextPitch, nextYaw, 0);
    }
}