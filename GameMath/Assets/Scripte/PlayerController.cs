using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    [HideInInspector] public bool isLeftParrying = false;
    [HideInInspector] public bool isRightParrying = false;

    void Update()
    {
        Move();
        Rotate();
        HandleParry();
    }

    void Move()
    {
        float vertical = Input.GetAxis("Vertical"); // W/S 또는 위/아래
        Vector3 move = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void Rotate()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D 또는 좌/우
        float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);
    }

    void HandleParry()
    {
        // 매 프레임 초기화
        isLeftParrying = false;
        isRightParrying = false;

        if (Input.GetKey(KeyCode.Q))
        {
            isLeftParrying = true;
        }

        if (Input.GetKey(KeyCode.E))
        {
            isRightParrying = true;
        }
    }
}
