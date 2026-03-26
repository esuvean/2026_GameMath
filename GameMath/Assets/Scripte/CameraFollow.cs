using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 6f, -8f);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 rotatedOffset = Quaternion.Euler(0f, target.eulerAngles.y, 0f) * offset;
        transform.position = target.position + rotatedOffset;
        transform.LookAt(target);
    }
}
