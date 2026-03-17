using TMPro;
using UnityEngine;

public class SimpleAngleLauncher : MonoBehaviour
{
    public TMP_InputField angleInputFirld;
    public GameObject spherePerefab;
    public Transform firePoint;
    public float forece = 15f;

    public void Launch()
    {
        float angle = float.Parse(angleInputFirld.text);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
        Debug.Log($"Angle: {angle}°, Direction: {dir}");
        GameObject gameObject = Instantiate(spherePerefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = spherePerefab.GetComponent<Rigidbody>();

        rb.AddForce((dir + Vector3.up * .3f).normalized * forece, ForceMode.Impulse);
    }
}
