using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpTest : MonoBehaviour
{
    float t = 0f;

    CustomTransform cTrans;

    CustomQuaternion tiltQuaternion;
    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();


    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * 2f;

        CustomQuaternion tilt = new CustomQuaternion(20, transform.right);

        tiltQuaternion = (tilt * new CustomQuaternion(transform.up) * tilt.Inverse());

        cTrans.Rotation = (tiltQuaternion).GetAxis();

        Debug.DrawLine(Vector3.zero, cTrans.Position + tiltQuaternion.GetAxis(), Color.black, 0.1f);
    }
}
