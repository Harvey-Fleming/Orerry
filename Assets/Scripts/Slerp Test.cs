using UnityEngine;

public class SlerpTest : MonoBehaviour
{
    float t = 0f;

    CustomTransform cTrans;

    [SerializeField] float tiltAngle;

    CustomQuaternion nextQuat;

    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * TimeManager.instance.TimeScale;

        nextQuat = new CustomQuaternion(t * Mathf.PI / 180, Vector3.up);

        CustomQuaternion pos = new(new Vector3(5,0,0));

        cTrans.Position = (nextQuat * pos * nextQuat.Inverse()).GetAxis();

    }

}
