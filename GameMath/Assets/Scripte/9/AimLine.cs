using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLine : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float extend = 1.5f;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.red;
        lr.endColor = Color.red;
    }

    void Update()
    {
        if (startPos == null || endPos == null) return;

        Vector3 a = startPos.position;
        Vector3 b = endPos.position;

        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);

        lr.SetPosition(0, a);
        lr.SetPosition(1, pred);
    }
}