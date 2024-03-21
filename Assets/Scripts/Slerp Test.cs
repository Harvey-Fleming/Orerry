using UnityEngine;

public class SlerpTest : MonoBehaviour
{
    float t = 0f;

    CustomTransform cTrans;

    [SerializeField] float tiltAngle;

    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * 2f;

        CustomQuaternion tilt = new CustomQuaternion(t, MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180));

        Debug.Log(MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180));

        cTrans.AdditionalMatrix = tilt.ConvertToMatrix();
        Debug.Log("Rotations should be " + tilt.ToEulerAngles());

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(cTrans.Position , cTrans.Position + new Vector3(MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180).x, MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180).y, 0));
    }
}
