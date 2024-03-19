using UnityEngine;

public class SlerpTest : MonoBehaviour
{
    float t = 0f;

    CustomTransform cTrans;

    Matrix4by4 tiltRotMatrix;

    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * 2f;

        //CustomQuaternion tilt = new CustomQuaternion(t, MathLib.RadiansToVector((20) * Mathf.PI / 180));
        CustomQuaternion tilt = new CustomQuaternion(t, MathLib.EulerAnglestoDirection(new Vector3((20) * Mathf.PI / 180, 0, 0)));

        Debug.Log(MathLib.EulerAnglestoDirection(new Vector3((20) * Mathf.PI / 180, 0, 0)));

        cTrans.Rotation = tilt.ToEulerAngles();

        Debug.DrawLine(Vector3.zero, MathLib.EulerAnglestoDirection(new Vector3((20) * Mathf.PI / 180, 0, 0)), Color.black, 0.1f);
    }
}
