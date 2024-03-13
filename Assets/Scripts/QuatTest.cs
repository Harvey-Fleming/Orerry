using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatTest : MonoBehaviour
{
    float t = 0;

    // Update is called once per frame
    void Update()
    {
            t += Time.deltaTime * 0.005f;

            CustomQuaternion q = new CustomQuaternion(1, new Vector3(0,1,0));
            CustomQuaternion r = new CustomQuaternion(15, new Vector3(1,0,0));

            CustomQuaternion slerped = CustomQuaternion.Slerp(q,r,t);

            CustomQuaternion k = new CustomQuaternion(GetComponent<CustomTransform>().Position);

            CustomQuaternion newK = slerped * k * slerped.Inverse();


            Vector3 newP = newK.GetAxis();
            //Debug.Log(newP);

            GetComponent<CustomTransform>().Position = newP;
    }
}
