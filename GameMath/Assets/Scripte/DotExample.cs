using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;

        Vector3 forward = transform.forward;
        forward.y = 0;

        forward.Normalize();
        toPlayer.Normalize();

        float dot = Vector3.Dot(forward, toPlayer);

        if (dot > 0f)
        {
            Debug.Log("플레이어가 적의 앞쪽에 있음");
        }
        else if (dot < 0f)
        {
            Debug.Log("플레이어가 적의 뒤쪽에 있음");

        }
        else
        {
            Debug.Log("플레이어가 적의 옆쪽에 있음");
        }
    }
}
