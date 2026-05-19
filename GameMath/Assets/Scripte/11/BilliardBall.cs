using UnityEngine;

public enum BallType
{
    Player1,
    Player2,
    Target
}

public class BilliardBall : MonoBehaviour
{
    public BallType ballType;

    private BilliardsGameManager manager;

    void Start()
    {
        manager = FindObjectOfType<BilliardsGameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        BilliardBall otherBall = collision.gameObject.GetComponent<BilliardBall>();

        if (otherBall != null && manager != null)
        {
            manager.ReportBallHit(this, otherBall);
        }
    }
}