using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDistance = 3f;
    public float jumpForce = 5f;

    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    private bool isDashing = false;

    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;

    private bool isMoving = false;
    private bool isSprint = false;

    private Vector3 lastMoveDirection;
    private bool hasMoved = false;

    private Rigidbody rb;
    private bool isGrounded = true;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>(); // 마우스 위치 업데이트
    }
    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray); // 레이저 경로에 있는 모든 물체를 탐색

            foreach (RaycastHit hit in hits ) // 모든 물체에 한에 반복
            {
                if (hit.collider.gameObject != gameObject) // 부딪힌 물체가 나 자신이 아닐 때만
                {
                    targetPosition = hit.point; // Plane에 부딪힌 지점을 타겟
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break; // 탐색 했으니 foreach 반복 중단
                }
            }
        }
    }
    public void OnSprint(InputValue value)
    {
        isSprint = value.isPressed;
    }
    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    public void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            isMoving = false;

            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 dashDirection = hit.point - transform.position;
                dashDirection.y = 0f;

                float magnitude = Mathf.Sqrt(
                    dashDirection.x * dashDirection.x +
                    dashDirection.y * dashDirection.y +
                    dashDirection.z * dashDirection.z
                );

                if (magnitude > 0.01f)
                {
                    dashDirection = dashDirection / magnitude;
                    StartCoroutine(Dash(dashDirection));
                }
            }
        }
    }
    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        isMoving = false;

        float timer = 0f;

        while (timer < dashTime)
        {
            transform.Translate(direction * dashSpeed * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            float magnitudeSquared = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
            float magnitude = Mathf.Sqrt(magnitudeSquared);

            if (magnitude > 0.01f)
            {
                Vector3 normalizedDirection = direction / magnitude; 

                lastMoveDirection = normalizedDirection; // 마지막 이동 방향 저장
                hasMoved = true;

                float speed = moveSpeed; 
                if (isSprint)
                    speed *= 2f;
                transform.Translate(normalizedDirection * speed * Time.deltaTime, Space.World);
            }
            else
            {
                isMoving = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
