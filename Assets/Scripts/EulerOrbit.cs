using UnityEngine;

[RequireComponent(typeof(CustomTransform))]
public class EulerOrbit : MonoBehaviour
{
    [Header("Planet Information")]
    [SerializeField] private Planet planetInformation;

    [Header("Orbit Variables")]
    [Tooltip("This is the Game Object that holds the planet this gameobject will orbit around")]
    [SerializeField] private GameObject primaryBody;

    [SerializeField] private float orbitRadius = 5f;
    [Space]
    [SerializeField] private float tiltAngle = 20f;

    CustomTransform cTrans;
    float t = 0f;

    float amountToRotate;
    float orbitRotationCooldown = 0f;
    
    [SerializeField] private float orbitalPeriod = 1f;
    public Planet PlanetInformation { get => planetInformation;}
    public float OrbitRadius { get => orbitRadius;}
    public GameObject PrimaryBody { get => primaryBody;}

    // Start is called before the first frame update
    void Start()
    {
        cTrans = GetComponent<CustomTransform>();

        cTrans.Position = new Vector3(orbitRadius,0,0);
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

            //Calculate how much we should rotate by based on the time manager sim second
            amountToRotate = 360 / (TimeManager.instance.SimulationSecond * (orbitalPeriod *24 * 60 * 60));

            Vector3 newP = MyVector3.RotateVector(cTrans.Position, amountToRotate, Vector3.up);

            cTrans.Position = newP + primaryBody.GetComponent<CustomTransform>().Position;

            orbitRotationCooldown = 0f;
        }
        else if(primaryBody != null && orbitRotationCooldown < 1)
        {
            orbitRotationCooldown += 1 * Time.deltaTime;
        }

    }
}
