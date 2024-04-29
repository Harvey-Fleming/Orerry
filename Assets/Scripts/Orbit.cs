using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(CustomTransform))]
public class Orbit : MonoBehaviour
{
    [Header("Planet Information")]
    [SerializeField] private Planet planetInformation;

    [Header("Orbit Variables")]
    [Tooltip("This is the Game Object that holds the planet this gameobject will orbit around")]
    [SerializeField] private GameObject primaryBody;

    [SerializeField] private float orbitRadius = 5f;
    [Space]
    [SerializeField] private float tiltAngle = 20f;
    
    private CustomQuaternion nextQuat;
    private CustomQuaternion startSlerpQuat;

    CustomTransform cTrans;
    float t = 0f;

    float amountToRotate = 0f;
    float prevamountToRotate = 0f;
    float orbitRotationCooldown = 1.0f;
    
    [SerializeField] private float orbitalPeriod = 1f;
    [Space]
    [SerializeField] private bool isDebug;


    public Planet PlanetInformation { get => planetInformation;}
    public float OrbitRadius { get => orbitRadius;}
    public GameObject PrimaryBody { get => primaryBody;}

    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();

        cTrans.Rotation = new Vector3(cTrans.Rotation.x, cTrans.Rotation.y, -tiltAngle);

    }

    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime * 2f;

        CustomQuaternion tilt = new CustomQuaternion(t, MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180));
        Debug.DrawRay(cTrans.Position, MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180), Color.white, 0.01f);
        cTrans.AdditionalMatrix = tilt.ConvertToMatrix();

        if (primaryBody != null && orbitRotationCooldown >= 1)
        {
            //Reset Timer
            orbitRotationCooldown = 0f;

            //This will be the rotation that will be the start point of the slerp
            prevamountToRotate = amountToRotate;

            //Calculate how much we should rotate by based on the time manager sim second
            amountToRotate += 360 / (TimeManager.instance.SimulationSecond * (orbitalPeriod *24 * 60 * 60));

            //This is the quaternion holding the desired rotation
            nextQuat = new CustomQuaternion(amountToRotate * Mathf.PI/180, Vector3.up);

            //This holds the quaternion with the original rotation.
            startSlerpQuat = new CustomQuaternion(prevamountToRotate * Mathf.PI/180, Vector3.up);

            if (isDebug) Debug.DrawRay(cTrans.Position, Vector3.up * 5, Color.magenta, Mathf.Infinity);

        }
        else if(primaryBody != null && orbitRotationCooldown < 1)
        {
            //This is increasing the timer
            orbitRotationCooldown += 1 * Time.deltaTime;

            //This is the position to be rotated
            CustomQuaternion k = new CustomQuaternion(new Vector3(orbitRadius, 0, 0));

            //This is the slerped value from the start orientation to the target orientation
            CustomQuaternion slerpValue = CustomQuaternion.Slerp(startSlerpQuat, nextQuat, orbitRotationCooldown);

            //This rotates the position to where it needs to be.
            Vector3 newPos = (slerpValue * k * slerpValue.Inverse()).GetAxis();

            //This sets the position to where it needs to be in the orbit and offsets it by it's parent planet's position.
            cTrans.Position = newPos + primaryBody.GetComponent<CustomTransform>().Position;
        }

    }
}
