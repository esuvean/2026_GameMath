using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public Transform target;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shooting();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Shooting();
        }
    }

    void Shooting()
    {
        for (int i = 0; i < 10; i++)
        {
            Bezier bezier = Instantiate(
                bullet,
                firePoint.position,
                Quaternion.identity
            ).GetComponent<Bezier>();

            bezier.p0 = firePoint;
            bezier.p3 = target;
            bezier.StartShooting();
        }
    }
}