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
        if (Input.GetMouseButtonDown(0) & !isZoomed)
        {
            Debug.Log("Ray Direction is " + ray.direction);

            BoxCollider[] colliders = FindObjectsOfType<BoxCollider>();
            BoxCollider closesthitCollider = null;
            float closestDist = Mathf.Infinity;

            foreach (BoxCollider collider in colliders)
            {
                if (AABB.LineIntersection(collider.AABBCollider, ray.origin, ray.direction * 1000 , out Vector3 intersectPoint))
                {
                    Debug.DrawLine(ray.origin, (ray.direction * 1000), Color.magenta, 5f);
                    float Dist = new MyVector3(collider.GetComponent<CustomTransform>().Position - transform.position).GetLength();
                    Debug.Log(collider.name + " is " + Dist);
                    if (Dist < closestDist)
                    {
                        closestDist = Dist;
                        closesthitCollider = collider;
                        Debug.Log(collider.name + " is now the closest");

                    }
                }
            }

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
            isZoomed = false;
            PlanetShowcaseUI.instance.HideCanvas();
            target = defaultTarget;
        }

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

        Vector3 dir = MyVector3.SubtractVectors(new MyVector3(targetTransform.Position), new MyVector3(transform.position)).ConvertToUnityVector();

        //Calculate forward direction
        Vector3 forwardDirection = dir;
        //Debug.DrawRay(transform.position, forwardDirection, Color.red, 0.1f);

        //Calculate right direction
        Vector3 rightDirection = MathLib.VectorCrossProduct(Vector3.up, forwardDirection);
        //Debug.DrawRay(transform.position, rightDirection, Color.blue, 0.1f);

        Vector3 updirection = MathLib.VectorCrossProduct(forwardDirection, rightDirection);
        //Debug.DrawRay(transform.position, updirection, Color.yellow, 0.1f);

        float pitchangle = 180 - Mathf.Atan2(-forwardDirection.y, forwardDirection.z) * 180 / Mathf.PI;
        float yawangle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * 180 / Mathf.PI;

        float nextPitch = MathLib.FloatLerp(transform.eulerAngles.x, pitchangle, Time.deltaTime * lerpSpeed);

        //Causes Strange Spin when Lerping
        //float nextYaw = MathLib.FloatLerp(transform.eulerAngles.y, yawangle, Time.deltaTime);

        transform.rotation = Quaternion.Euler(nextPitch, yawangle, transform.rotation.z);
    }
}
