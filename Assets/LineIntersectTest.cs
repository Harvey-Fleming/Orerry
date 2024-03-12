using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIntersectTest : MonoBehaviour
{
    [SerializeField] Vector3 lineIntersectStartPos;
    [SerializeField] Vector3 lineIntersectEndPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LineTest());
    }

    private IEnumerator LineTest()
    {
        while (true)
        {
            Debug.DrawRay(lineIntersectStartPos, lineIntersectEndPos, Color.red, 0.75f);
            if (AABB.LineIntersection(FindObjectOfType<BoxCollider>().AABBCollider, lineIntersectStartPos, lineIntersectEndPos, out Vector3 intersectionPoint))
            {
                Debug.Log("Collision at " + intersectionPoint);
            }

            lineIntersectEndPos.x += 1;
            yield return new WaitForSeconds(1f);
        }
    }

}
