using UnityEngine;

public class CameraSlerp : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;

    void Update()
    {
        if (target == null) 
            return;

        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);

        float t = 1f - Mathf.Exp(-speed * Time.deltaTime);

        transform.rotation = ManualSlerp(transform.rotation, lookRot, t);
    }

    Quaternion ManualSlerp(Quaternion from, Quaternion to, float t)
    {
        float dot = Quaternion.Dot(from, to);

        if (dot < 0f)
        {
            to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            dot = -dot;
        }

        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        if (1f - dot < 0.01f)
        {
            Quaternion lerp = new Quaternion(
                Mathf.Lerp(from.x, to.x, t),
                Mathf.Lerp(from.y, to.y, t),
                Mathf.Lerp(from.z, to.z, t),
                Mathf.Lerp(from.w, to.w, t)
            );

            return lerp.normalized;
        }

        float ratioA = Mathf.Sin((1f - t) * theta) / sinTheta;
        float ratioB = Mathf.Sin(t * theta) / sinTheta;

        Quaternion result = new Quaternion(
            ratioA * from.x + ratioB * to.x,
            ratioA * from.y + ratioB * to.y,
            ratioA * from.z + ratioB * to.z,
            ratioA * from.w + ratioB * to.w
        );

        return result.normalized;
    }
}