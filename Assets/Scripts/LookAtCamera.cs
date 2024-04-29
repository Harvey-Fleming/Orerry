using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject defaultTarget;

    [SerializeField] float defaultFOV;
    [SerializeField] float ZoomedFOV;


    [SerializeField] float lerpSpeed = 1f;

    Camera mainCamera;

    bool isZoomed = false;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtTarget();

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.direction * 1000, Color.green, 0.1f);
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Ray Direction is " + ray.direction);

            BoxCollider[] colliders = FindObjectsOfType<BoxCollider>();
            BoxCollider closesthitCollider = null;
            float closestDist = Mathf.Infinity;

            foreach (BoxCollider collider in colliders)
            {
                //Check if the ray from the camera collides with this Box Collider
                if (AABB.LineIntersection(collider.AABBCollider, ray.origin, ray.direction * 1000 , out Vector3 intersectPoint))
                {
                    Debug.DrawLine(ray.origin, (ray.direction * 1000), Color.magenta, 5f);
                    float Dist = new MyVector3(collider.GetComponent<CustomTransform>().Position - transform.position).GetLength();
                    Debug.Log(collider.name + " is " + Dist);
                    //Check if the collider is the closest one, If so assign it to the closest collider variable
                    if (Dist < closestDist)
                    {
                        closestDist = Dist;
                        closesthitCollider = collider;
                        Debug.Log(collider.name + " is now the closest");

                    }
                }
            }

            //If there is a closest collider, select that one
            if(closesthitCollider != null)
            {
                target = closesthitCollider.gameObject;
                if (target.GetComponent<Orbit>().PlanetInformation != null)
                {
                    isZoomed = true;
                    PlanetShowcaseUI.instance.ShowCanvas();
                    PlanetShowcaseUI.instance.SetUIInformation(target);
                }
            }

        }
        else if(isZoomed && Input.GetMouseButtonDown(1))
        {
            //Unselects Planet and being to return the FOV back to normal
            isZoomed = false;
            PlanetShowcaseUI.instance.HideCanvas();
            target = defaultTarget;
        }

        //Lerping Camera FOV
        if(isZoomed)
        {
            float nextFOV = MathLib.FloatLerp(mainCamera.fieldOfView, ZoomedFOV, Time.deltaTime);
            mainCamera.fieldOfView = nextFOV;
        }
        else
        {
            float nextFOV = MathLib.FloatLerp(mainCamera.fieldOfView, defaultFOV, Time.deltaTime);
            mainCamera.fieldOfView = nextFOV;
        }
    }

    private void LookAtTarget()
    {
        CustomTransform targetTransform = target.GetComponent<CustomTransform>();

        //Calculate directions based on camera orientation
        Vector3 forwardDirection = MyVector3.SubtractVectors(new MyVector3(targetTransform.Position), new MyVector3(transform.position)).ConvertToUnityVector();
        //Debug.DrawRay(transform.position, forwardDirection, Color.red, 0.1f);

        Vector3 rightDirection = MathLib.VectorCrossProduct(Vector3.up, forwardDirection);
        //Debug.DrawRay(transform.position, rightDirection, Color.blue, 0.1f);

        Vector3 updirection = MathLib.VectorCrossProduct(forwardDirection, rightDirection);
        //Debug.DrawRay(transform.position, updirection, Color.yellow, 0.1f);

        float pitchangle = 180 - Mathf.Atan2(-forwardDirection.y, forwardDirection.z) * 180 / Mathf.PI;
        float yawangle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * 180 / Mathf.PI;

        float nextPitch = MathLib.FloatLerp(transform.eulerAngles.x, pitchangle, Time.deltaTime * lerpSpeed);

        //Causes Strange Spin when Lerping
        //float nextYaw = MathLib.FloatLerp(transform.eulerAngles.y, yawangle, Time.deltaTime);

        //Using Quaternion.Euler as Unity's Transform uses quaternion under the hood so the euler angles must be converted into a quaternion.
        transform.rotation = Quaternion.Euler(nextPitch, yawangle, transform.rotation.z);
    }
}
