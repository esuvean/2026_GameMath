using System.Collections.Generic;
using UnityEngine;

public class StatsExample : MonoBehaviour
{
    [Header("Sample Settings")]
    public int sampleCount = 10;
    public float min = 0f;
    public float max = 100f;

    List<float> samples = new List<float>();

    void Start()
    {
        GenerateSamples();

        float mean = CalculateMean(samples);
        float std = CalculateStandardDeviation(samples, mean);

        Debug.Log("Mean : " + mean);
        Debug.Log("Standard Deviation : " + std);
    }

    void GenerateSamples()
    {
        samples.Clear();

        for (int i = 0; i < sampleCount; i++)
        {
            float value = Random.Range(min, max);
            samples.Add(value);
        }
    }

    float CalculateMean(List<float> data)
    {
        float sum = 0f;

        foreach (float v in data)
        {
            sum += v;
        }

        return sum / data.Count;
    }

    float CalculateStandardDeviation(List<float> data, float mean)
    {
        float sum = 0f;

        foreach (float v in data)
        {
            sum += Mathf.Pow(v - mean, 2);
        }

        return Mathf.Sqrt(sum / data.Count);
    }
}