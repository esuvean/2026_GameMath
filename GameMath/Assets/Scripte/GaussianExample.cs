using UnityEngine;

public class GaussianExample : MonoBehaviour
{
    public float mean = 50f;
    public float stdDev = 10f;

    public void GenerateValue()
    {
        float value = GaussianRandom(mean, stdDev);
        Debug.Log("Generated Value : " + value);
    }

    float GaussianRandom(float mean, float stdDev)
    {
        float u1 = Random.Range(0f, 1f);
        float u2 = Random.Range(0f, 1f);

        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) *
                  Mathf.Cos(2f * Mathf.PI * u2);

        return mean + z * stdDev;
    }
}