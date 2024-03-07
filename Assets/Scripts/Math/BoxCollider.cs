using UnityEngine;

public class BoxCollider : MonoBehaviour
{
    private AABB AABBcollider;

    public AABB AABBCollider { get => AABBcollider; set => AABBcollider = value; }

    // Update is called once per frame
    void Update()
    {
        AABBcollider = new AABB(new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f));
    }
}
