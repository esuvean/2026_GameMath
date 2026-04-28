using UnityEngine;

public class EnemyPingPong : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(3f, 0f, 0f);
    public float speed = 1.5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPos, startPos + moveOffset, t);
    }
}