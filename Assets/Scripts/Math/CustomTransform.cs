using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class CustomTransform : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale = Vector3.one;

    [Space]
    [SerializeField] private Mesh OriginalMesh;

    private Vector3[] modelSpaceVertices;

    private MeshFilter meshFilter;
    private Mesh mesh;

    Matrix4by4 M = Matrix4by4.Identity;
    Matrix4by4 inverseM = Matrix4by4.Identity;

    Matrix4by4 additionalMatrix = Matrix4by4.Identity;

    Vector3 updirection;
    Vector3 rightDirection;
    Vector3 forwardDirection;

    public Vector3 Position { get => position; set => position = value; }
    public Vector3 Rotation { get => rotation; set => rotation = value; }
    public Vector3 Scale { get => scale; set => scale = value; }
    public Matrix4by4 Matrix { get => M; }
    public Matrix4by4 AdditionalMatrix { get => additionalMatrix; set => additionalMatrix = value; }
    public Matrix4by4 InverseM { get => inverseM;}
    public Vector3 Updirection { get => updirection;}
    public Vector3 RightDirection { get => rightDirection;}
    public Vector3 ForwardDirection { get => forwardDirection;}


    // Start is called before the first frame update
    void Awake()
    {

        meshFilter = GetComponent<MeshFilter>();

        if (OriginalMesh == null)
        {
            OriginalMesh = meshFilter.sharedMesh;
        }

        Mesh meshCopy = Mesh.Instantiate(OriginalMesh);
        mesh = meshFilter.mesh = meshCopy;
        //Get the model vertices
        modelSpaceVertices = mesh.vertices;
    }


    // Update is called once per frame
    void Update()
    {
        Matrix4by4 R, scaleMatrix, translationMatrix;

        Vector3[] transformedVertices = new Vector3[modelSpaceVertices.Length];

        Matrix4by4 rollMatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(Rotation.z), Mathf.Sin(Rotation.z), 0),
            new Vector3(-Mathf.Sin(Rotation.z), Mathf.Cos(Rotation.z), 0),
            new Vector3(0, 0, 1),
            Vector3.zero);

        Matrix4by4 pitchMatrix = new Matrix4by4(
            new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(Rotation.x), Mathf.Sin(Rotation.x)),
            new Vector3(0, -Mathf.Sin(Rotation.x), Mathf.Cos(Rotation.x)),
            Vector3.zero);

        Matrix4by4 yawMatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(Rotation.y), 0, -Mathf.Sin(Rotation.y)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(Rotation.y), 0, Mathf.Cos(Rotation.y)),
            Vector3.zero);

        R = additionalMatrix * (yawMatrix * (pitchMatrix * rollMatrix));
        scaleMatrix = new Matrix4by4(new Vector3(1, 0, 0) * Scale.x, new Vector3(0, 1, 0) * Scale.y, new Vector3(0, 0, 1) * Scale.z, Vector3.zero);
        translationMatrix = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), Position);
        M = translationMatrix * (R * scaleMatrix);
        for (int i = 0; i < transformedVertices.Length; i++)
        {
            transformedVertices[i] = M * new Vector4(modelSpaceVertices[i].x, modelSpaceVertices[i].y, modelSpaceVertices[i].z, 1);
        }

        mesh.vertices = transformedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


        //Calculate forward direction
        Vector3 eulerAngle = rotation;

        forwardDirection = MathLib.EulerAnglestoDirection(eulerAngle);
        //Debug.DrawRay(position, forwardDirection, Color.red, 0.1f);

        //Calculate right direction
        rightDirection = MathLib.VectorCrossProduct(Vector3.up, forwardDirection);
        //Debug.DrawRay(position, rightDirection, Color.blue, 0.1f);

        updirection = MathLib.VectorCrossProduct(forwardDirection, rightDirection);
        //Debug.DrawRay(position, updirection, Color.yellow, 0.1f);

        inverseM = scaleMatrix.ScaleInverse() * (R.RotationInverse() * translationMatrix.TranslationInverse());

    }
}
