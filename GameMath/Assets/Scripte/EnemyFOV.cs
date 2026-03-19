using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;
    public float viewDistance = 7f;

    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance <= viewDistance)
        {
            toPlayer.Normalize();

            Vector3 forward = transform.forward;

            float dot =
                forward.x * toPlayer.x +
                forward.y * toPlayer.y +
                forward.z * toPlayer.z;

            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angle <= viewAngle / 2f)
            {
                transform.localScale = Vector3.one * 2f;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}