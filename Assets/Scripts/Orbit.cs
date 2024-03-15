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
    [SerializeField] private Vector3 orbitalRotation;

    CustomTransform cTrans;
    private float t;

    private float yawAngle = 0f;
    [SerializeField] private float rotationSpeed = 1f;

    public Vector3 OrbitalRotation { get => orbitalRotation; set => orbitalRotation = value; }
    public Planet PlanetInformation { get => planetInformation; set => planetInformation = value; }

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

        
        if(primaryBody != null)
        {
            yawAngle += Time.deltaTime * rotationSpeed * TimeManager.instance.TimeScale;

            CustomQuaternion q = new CustomQuaternion(yawAngle, primaryBody.GetComponent<CustomTransform>().Updirection);

            CustomQuaternion k = new CustomQuaternion(new Vector3(orbitRadius, 0, 0));

            CustomQuaternion newK = q * k * q.Inverse();

            Vector3 newP = newK.GetAxis();
            //Debug.Log(newP);

            cTrans.Position = newP + primaryBody.GetComponent<CustomTransform>().Position;
            
            //Slerp to that angle >:)
        }

    }
}
