using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PredictionLineRender : MonoBehaviour
{
    public Transform startPos; // A (보통 카메라 or 플레이어)
    public Transform endPos;   // 타겟

    [Range(1f, 5f)]
    public float extend = 1.5f;

    private LineRenderer lr;
    public CameraSlerp cs;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = Color.red;
        lr.enabled = false; // 처음엔 안보이게
    }

    public void OnRightCilck(InputValue value)
    {
        // 👉 우클릭 타게팅
        {
            if (!value.isPressed) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    // 타게팅
                    startPos = transform;
                    endPos = hit.collider.transform;
                    lr.enabled = true;
                    cs.target = endPos;
                }
                else
                {
                    // 초기화
                    endPos = null;
                    lr.enabled = false;
                }
            }
            else
            {
                // 아무것도 안 맞으면 초기화
                endPos = null;
                lr.enabled = false;
            }
        }

        // 👉 라인 업데이트
        if (!startPos || !endPos) return;

        Vector3 a = startPos.position;
        Vector3 b = endPos.position;

        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);

        lr.SetPosition(0, a);
        lr.SetPosition(1, pred);
    }
}