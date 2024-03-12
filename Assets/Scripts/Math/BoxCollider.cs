using UnityEngine;

[RequireComponent(typeof(CustomTransform))]
public class BoxCollider : MonoBehaviour
{
    [SerializeField] private Vector3 colliderSize;
    private AABB AABBcollider;

    public AABB AABBCollider { get => AABBcollider;}

    private void Awake()
    {
        AABBcollider = new AABB(GetComponent<CustomTransform>().Position - colliderSize, GetComponent<CustomTransform>().Position + colliderSize);

    }

    // Update is called once per frame
    void Update()
    {
        AABBcollider = new AABB(GetComponent<CustomTransform>().Position - colliderSize,GetComponent<CustomTransform>().Position + colliderSize);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(GetComponent<CustomTransform>().Position, new Vector3(colliderSize.x, colliderSize.y, colliderSize.z));
    }
}
