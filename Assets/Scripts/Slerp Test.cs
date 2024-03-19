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

        CustomQuaternion tilt = new CustomQuaternion(t, MathLib.RadiansToVector((20 + 90) * Mathf.PI / 180));
        //CustomQuaternion tilt = new CustomQuaternion(t, MathLib.EulerAnglestoDirection(new Vector3((20) * Mathf.PI / 180, 0, 0)));

        Debug.Log(MathLib.RadiansToVector((20 + 90) * Mathf.PI / 180));

        //cTrans.Rotation = tilt.ToEulerAngles();
        cTrans.AdditionalMatrix = tilt.ConvertToMatrix();
        Debug.Log("Rotations should be " + tilt.ToEulerAngles());
        //cTrans.AdditionalMatrix = tilt.ConvertToMatrix();

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(Vector3.zero, MathLib.RadiansToVector((20 + 90) * Mathf.PI / 180) * 20);
    }
}
