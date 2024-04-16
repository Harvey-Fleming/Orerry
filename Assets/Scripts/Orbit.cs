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
    [SerializeField] private bool isDebug;
    CustomTransform cTrans;
    float t = 0f;

    float amountToRotate = 0f;
    float prevamountToRotate = 0f;
    float orbitRotationCooldown = 1.0f;
    
    [SerializeField] private float orbitalPeriod = 1f;
    public Planet PlanetInformation { get => planetInformation;}
    public float OrbitRadius { get => orbitRadius;}
    public GameObject PrimaryBody { get => primaryBody;}

    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        #region - Old Method
        //if(primaryBody != null)
        //{

        //    yawAngle += Time.deltaTime * TimeManager.instance.TimeScale;
        //    orbitalRotation = new Vector3(0, yawAngle, 0);

        //    CustomTransform parentTrans = primaryBody.GetComponent<CustomTransform>();
        //    cTrans.Position = new Vector3(parentTrans.Position.x + orbitRadius, parentTrans.Position.y, parentTrans.Position.z);
        //    cTrans.TranslateRotateScale();

        //    if(primaryBody.GetComponent<Orbit>().primaryBody != null)
        //    {
        //        cTrans.Rotation = primaryBody.GetComponent<Orbit>().OrbitalRotation;
        //        cTrans.RotateAroundPoint(new Vector3(cTrans.Rotation.x, cTrans.Rotation.y, cTrans.Rotation.z), Vector3.zero);
        //    }
        //    else
        //    {
        //        cTrans.RotateAroundPoint(new Vector3(cTrans.Rotation.x, yawAngle, cTrans.Rotation.z), parentTrans.Position);
        //    }
        //}
        #endregion

        t += Time.deltaTime * 2f;

        CustomQuaternion tilt = new CustomQuaternion(t, MathLib.RadiansToVector((tiltAngle + 90) * Mathf.PI / 180));
        cTrans.AdditionalMatrix = tilt.ConvertToMatrix();

        if (primaryBody != null && orbitRotationCooldown >= 1)
        {
            //Reset Timer
            orbitRotationCooldown = 0f;

            prevamountToRotate = amountToRotate;
            //Calculate how much we should rotate by based on the time manager sim second
            amountToRotate += 360 / (TimeManager.instance.SimulationSecond * (orbitalPeriod *24 * 60 * 60));

            //Do the quaternion stuff
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
            if (isDebug) Debug.Log("The new Pos for " + name +  " should be: " + newPos);

            //This sets the position to where it needs to be in the orbit and offsets it by it's parent planet's position.
            cTrans.Position = newPos + primaryBody.GetComponent<CustomTransform>().Position;
            //cTrans.Position = (nextQuat * k * nextQuat.Inverse()).GetAxis() + primaryBody.GetComponent<CustomTransform>().Position;
            if (isDebug) Debug.Log(cTrans.Position);
        }

    }
}
