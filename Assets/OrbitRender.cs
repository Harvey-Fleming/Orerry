using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)), RequireComponent(typeof(CustomTransform))]
[ExecuteAlways]
public class OrbitRender : MonoBehaviour
{
    float prevOrbitRadius;

    CustomTransform cTrans;
    LineRenderer lineRenderer;
    Orbit orbit;

    [SerializeField] private int subdivisions = 10;

    // Start is called before the first frame update
    void Start()
    {
        orbit = GetComponent<Orbit>();
        lineRenderer = GetComponent<LineRenderer>();
        cTrans = GetComponent<CustomTransform>();

        lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float angleStep = 2f * Mathf.PI / subdivisions;

        lineRenderer.positionCount = subdivisions;

        //Do the quaternion stuff
        for (int i = 0; i < subdivisions; i++)
        {
            Vector3 point = new Vector3(Mathf.Cos(angleStep * i) * orbit.OrbitRadius, 0, (Mathf.Sin(angleStep * i) * orbit.OrbitRadius));

            lineRenderer.SetPosition(i, point + orbit.PrimaryBody.GetComponent<CustomTransform>().Position);
        }

        prevOrbitRadius = orbit.OrbitRadius;
    }
}
