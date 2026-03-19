using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.forward = direction;
        }
    }
}