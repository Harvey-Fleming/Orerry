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
    AABB boundingBox;

    Matrix4by4 M = Matrix4by4.Identity;
    Matrix4by4 inverseM = Matrix4by4.Identity;

    public Vector3 Position { get => position; set => position = value; }
    public Vector3 Rotation { get => rotation; set => rotation = value; }
    public Vector3 Scale { get => scale; set => scale = value; }
    public Matrix4by4 Matrix { get => M;}
    public Matrix4by4 InverseM { get => inverseM; set => inverseM = value; }


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
        boundingBox = new AABB(new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f));
        Matrix4by4 R, scaleMatrix, translationMatrix;
        TranslateRotateScale(out R, out scaleMatrix, out translationMatrix);

        #region Line Intersection
        Vector3 GlobalStart = new Vector3(-2, -2, -2);
        Vector3 GlobalEnd = new Vector3(3, 4, 5);

        inverseM = scaleMatrix.ScaleInverse() * (R.RotationInverse() * translationMatrix.TranslationInverse());

        Vector3 LocalStart = InverseM * GlobalStart;
        Vector3 LocalEnd = InverseM * GlobalEnd;

        Vector3 intersectPoint;
        if (AABB.LineIntersection(boundingBox, LocalStart, LocalEnd, out intersectPoint))
        {
            //Debug.Log("Intersecting! Local Intersection Point " + intersectPoint);
            //Debug.Log("Global Intersection Point " + (M * intersectPoint));
        }
        #endregion
    }

    private void TranslateRotateScale(out Matrix4by4 R, out Matrix4by4 scaleMatrix, out Matrix4by4 translationMatrix)
    {
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

        R = (yawMatrix * (pitchMatrix * rollMatrix));
        scaleMatrix = new Matrix4by4(new Vector3(1, 0, 0) * Scale.x, new Vector3(0, 1, 0) * Scale.y, new Vector3(0, 0, 1) * Scale.z, Vector3.zero);
        translationMatrix = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), Position);
        M = translationMatrix * (R * scaleMatrix);
        //for (int i = 0; i < transformedVertices.Length; i++)
        //{
        //    transformedVertices[i] = M * new Vector4(modelSpaceVertices[i].x, modelSpaceVertices[i].y, modelSpaceVertices[i].z, 1);
        //}

        //mesh.vertices = transformedVertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void TranslateRotateScale()
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

        R = (yawMatrix * (pitchMatrix * rollMatrix));
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
    }

    public void RotateAroundPoint(Vector3 newRot, Vector3 newPos)
    {
        Matrix4by4 R, translationMatrix, mtranslationMatrix, returntranslationMatrix;
        Vector3[] transformedVertices = new Vector3[modelSpaceVertices.Length];

        Matrix4by4 rollMatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(newRot.z), Mathf.Sin(newRot.z), 0),
            new Vector3(-Mathf.Sin(newRot.z), Mathf.Cos(newRot.z), 0),
            new Vector3(0, 0, 1),
            Vector3.zero);

        Matrix4by4 pitchMatrix = new Matrix4by4(
            new Vector3(1, 0, 0),
            new Vector3(0, Mathf.Cos(newRot.x), Mathf.Sin(newRot.x)),
            new Vector3(0, -Mathf.Sin(newRot.x), Mathf.Cos(newRot.x)),
            Vector3.zero);

        Matrix4by4 yawMatrix = new Matrix4by4(
            new Vector3(Mathf.Cos(newRot.y), 0, -Mathf.Sin(newRot.y)),
            new Vector3(0, 1, 0),
            new Vector3(Mathf.Sin(newRot.y), 0, Mathf.Cos(newRot.y)),
            Vector3.zero);

        R = (yawMatrix * (pitchMatrix * rollMatrix));
        translationMatrix = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), newPos);
        mtranslationMatrix = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), -newPos);
        returntranslationMatrix = new Matrix4by4(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), position);
        M = (translationMatrix * (R * mtranslationMatrix)) * returntranslationMatrix;
        for (int i = 0; i < transformedVertices.Length; i++)
        {
            transformedVertices[i] = M * new Vector4(modelSpaceVertices[i].x, modelSpaceVertices[i].y, modelSpaceVertices[i].z, 1);
        }

        mesh.vertices = transformedVertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
