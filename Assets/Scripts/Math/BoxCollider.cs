using UnityEngine;

[RequireComponent(typeof(CustomTransform))]
public class BoxCollider : MonoBehaviour
{
    [SerializeField] private Vector3 colliderSize;
    private AABB AABBcollider;

    public AABB AABBCollider { get => AABBcollider; set => AABBcollider = value; }

    // Update is called once per frame
    void Update()
    {
        AABBcollider = new AABB(GetComponent<CustomTransform>().Position, new Vector3(colliderSize.x, colliderSize.y, colliderSize.z));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(GetComponent<CustomTransform>().Position, new Vector3(colliderSize.x, colliderSize.y, colliderSize.z));
    }
}
