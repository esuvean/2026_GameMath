using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovememt : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        transform.Translate(direction*moveSpeed*Time.deltaTime);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

}
