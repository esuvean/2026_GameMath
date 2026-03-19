using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public Transform sun;
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform moon;
    public Transform mars;
    public Transform jupiter;

    public float mercuryDistance = 3f;
    public float venusDistance = 5f;
    public float earthDistance = 7f;
    public float moonDistance = 1.2f;
    public float marsDistance = 9f;
    public float jupiterDistance = 12f;

    public float mercurySpeed = 2.5f;
    public float venusSpeed = 2f;
    public float earthSpeed = 1.5f;
    public float moonSpeed = 4f;
    public float marsSpeed = 1.2f;
    public float jupiterSpeed = 0.8f;

    private float mercuryAngle;
    private float venusAngle;
    private float earthAngle;
    private float moonAngle;
    private float marsAngle;
    private float jupiterAngle;

    void Update()
    {
        mercuryAngle += mercurySpeed * Time.deltaTime;
        venusAngle += venusSpeed * Time.deltaTime;
        earthAngle += earthSpeed * Time.deltaTime;
        moonAngle += moonSpeed * Time.deltaTime;
        marsAngle += marsSpeed * Time.deltaTime;
        jupiterAngle += jupiterSpeed * Time.deltaTime;

        mercury.position = sun.position + new Vector3(
            Mathf.Cos(mercuryAngle) * mercuryDistance,
            0f,
            Mathf.Sin(mercuryAngle) * mercuryDistance
        );

        venus.position = sun.position + new Vector3(
            Mathf.Cos(venusAngle) * venusDistance,
            0f,
            Mathf.Sin(venusAngle) * venusDistance
        );

        earth.position = sun.position + new Vector3(
            Mathf.Cos(earthAngle) * earthDistance,
            0f,
            Mathf.Sin(earthAngle) * earthDistance
        );

        moon.position = earth.position + new Vector3(
            Mathf.Cos(moonAngle) * moonDistance,
            0f,
            Mathf.Sin(moonAngle) * moonDistance
        );

        mars.position = sun.position + new Vector3(
            Mathf.Cos(marsAngle) * marsDistance,
            0f,
            Mathf.Sin(marsAngle) * marsDistance
        );

        jupiter.position = sun.position + new Vector3(
            Mathf.Cos(jupiterAngle) * jupiterDistance,
            0f,
            Mathf.Sin(jupiterAngle) * jupiterDistance
        );
    }
}

