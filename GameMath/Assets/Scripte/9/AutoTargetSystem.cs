using UnityEngine;

public class AutoTargetSystem : MonoBehaviour
{
    public Camera playerCamera;
    public LineRenderer aimLine;
    public float rotateSpeed = 5f;

    private Transform target;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        aimLine.positionCount = 2;

        // 조준선 두께
        aimLine.startWidth = 0.1f;
        aimLine.endWidth = 0.1f;

        // 조준선 색
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = Color.magenta;
        aimLine.endColor = Color.magenta;

        aimLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryTarget();
        }

        if (target != null)
        {
            LookAtTarget();
            DrawAimLine();
        }
    }

    void TryTarget()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("맞은 오브젝트: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("타겟 잡힘!");
                target = hit.collider.transform;
                aimLine.enabled = true;
                return;
            }
        }
        else
        {
            Debug.Log("아무것도 안 맞음");
        }

        Debug.Log("타게팅 해제");
        target = null;
        aimLine.enabled = false;
    }

    void LookAtTarget()
    {
        Vector3 direction = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * rotateSpeed
        );
    }

    void DrawAimLine()
    {
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, target.position);
    }
}