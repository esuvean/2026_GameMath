using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public Transform p0;
    public Transform p3;

    [Header("Random Ranges")]
    public float p1Radius = 2f;
    public float p2Radius = 2f;
    public float p1Height = 3f;
    public float p2Height = 3f;

    Vector3 p1;
    Vector3 p2;
    List<Vector3> points;

    float time = 0f;
    bool isShooting = false;

    public void StartShooting()
    {
        time = 0f;
        isShooting = true;

        GenerateRandomControlPoints();

        points = new List<Vector3>()
        {
            p0.position,
            p1,
            p2,
            p3.position
        };

        transform.position = p0.position;
    }

    void Update()
    {
        if (!isShooting) return;

        time += Time.deltaTime / 2f;
        time = Mathf.Clamp01(time);

        transform.position = DeCasteljau(points, time);

        if (time >= 1f)
        {
            Destroy(gameObject);
        }
    }

    void GenerateRandomControlPoints()
    {
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = p0.position + new Vector3(rand1.x, p1Height, rand1.y);

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = p3.position + new Vector3(rand2.x, p2Height, rand2.y);
    }

    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;
            var next = new List<Vector3>(last);

            for (int i = 0; i < last; i++)
            {
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            }

            p = next;
        }

        return p[0];
    }
}