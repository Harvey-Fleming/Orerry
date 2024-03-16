using UnityEngine;

[RequireComponent(typeof(CustomTransform))]
[ExecuteAlways]
public class BoxCollider : MonoBehaviour
{
    [SerializeField] private Vector3 colliderSize;
    private AABB AABBcollider;

    private CustomTransform Trans;

    public AABB AABBCollider { get => AABBcollider;}

    private void Awake()
    {
        Trans = GetComponent<CustomTransform>();
        AABBcollider = new AABB(GetComponent<CustomTransform>().Position - Trans.Scale, Trans.Position + Trans.Scale);

    }

    // Update is called once per frame
    void Update()
    {
        AABBcollider = new AABB(GetComponent<CustomTransform>().Position - Trans.Scale, Trans.Position + Trans.Scale);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(GetComponent<CustomTransform>().Position, Trans.Scale);
    }
}
