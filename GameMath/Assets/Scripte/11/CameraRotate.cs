using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform player1Ball;
    public Transform player2Ball;

    public float rotateSpeed = 80f;
    public Vector3 offset = new Vector3(0, 6, -8);

    private float yaw = 0f;

    private BilliardsGameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<BilliardsGameManager>();
    }

    void Update()
    {
        if (gameManager == null) return;

        Transform target = GetCurrentPlayerBall();

        if (target == null) return;

        float input = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            input = -1f;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            input = 1f;
        }

        yaw += input * rotateSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0, yaw, 0);

        transform.position = target.position + rotation * offset;

        transform.LookAt(target);
    }

    Transform GetCurrentPlayerBall()
    {
        if (gameManager.GetCurrentPlayer() == 1)
        {
            return player1Ball;
        }
        else
        {
            return player2Ball;
        }
    }
}