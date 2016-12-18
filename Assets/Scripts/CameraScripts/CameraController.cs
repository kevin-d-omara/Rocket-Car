using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 Offsets;

    public float lookDamping;

    void Start()
    {

    }


    void FixedUpdate()
    {
        Matrix4x4 tbn = new Matrix4x4();
        tbn.SetColumn(0, target.right);
        tbn.SetColumn(1, target.up);
        tbn.SetColumn(2, target.forward);
        tbn.SetColumn(3, new Vector4(1, 1, 1, 1));
        transform.position = Vector3.Lerp(transform.position, (Vector3)(tbn * Offsets) + (target.position), .3f);
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookDamping);
    }
}
