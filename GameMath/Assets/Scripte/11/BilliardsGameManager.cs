using System.Collections.Generic;
using UnityEngine;

public class BilliardsGameManager : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;

    [Header("Balls")]
    public BilliardBall player1Ball;
    public BilliardBall player2Ball;
    public BilliardBall[] targetBalls;

    [Header("Power")]
    public float minPower = 3f;
    public float maxPower = 15f;
    public float chargeSpeed = 10f;

    [Header("Stop Check")]
    public float stopSpeed = 0.1f;
    public float stopWaitTime = 1f;

    private int currentPlayer = 1;
    private int player1Score = 0;
    private int player2Score = 0;

    private bool isCharging = false;
    private bool isWaitingStop = false;
    private bool gameOver = false;

    private float currentPower;
    private float stoppedTimer = 0f;

    private BilliardBall selectedBall;
    private Vector3 hitPoint;

    private HashSet<BilliardBall> hitTargets = new HashSet<BilliardBall>();
    private bool hitOpponentBall = false;

    void Update()
    {
        if (gameOver) return;

        if (isWaitingStop)
        {
            CheckBallsStopped();
            return;
        }

        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectBall();
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            currentPower += chargeSpeed * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, minPower, maxPower);
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ShootBall();
        }
    }

    void TrySelectBall()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            BilliardBall ball = hit.collider.GetComponent<BilliardBall>();

            if (ball == null) return;

            if (currentPlayer == 1 && ball.ballType != BallType.Player1) return;
            if (currentPlayer == 2 && ball.ballType != BallType.Player2) return;

            selectedBall = ball;
            hitPoint = hit.point;

            currentPower = minPower;
            isCharging = true;

            hitTargets.Clear();
            hitOpponentBall = false;
        }
    }

    void ShootBall()
    {
        Rigidbody rb = selectedBall.GetComponent<Rigidbody>();

        Vector3 direction = selectedBall.transform.position - hitPoint;
        direction.y = 0f;
        direction.Normalize();

        rb.AddForce(direction * currentPower, ForceMode.Impulse);

        isCharging = false;
        isWaitingStop = true;
        stoppedTimer = 0f;
    }

    void CheckBallsStopped()
    {
        if (AllBallsStopped())
        {
            stoppedTimer += Time.deltaTime;

            if (stoppedTimer >= stopWaitTime)
            {
                FinishTurn();
            }
        }
        else
        {
            stoppedTimer = 0f;
        }
    }

    bool AllBallsStopped()
    {
        BilliardBall[] allBalls = FindObjectsOfType<BilliardBall>();

        foreach (BilliardBall ball in allBalls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();

            if (rb.velocity.magnitude > stopSpeed)
            {
                return false;
            }
        }

        return true;
    }

    public void ReportBallHit(BilliardBall a, BilliardBall b)
    {
        if (!isWaitingStop) return;

        BilliardBall myBall = currentPlayer == 1 ? player1Ball : player2Ball;
        BilliardBall opponentBall = currentPlayer == 1 ? player2Ball : player1Ball;

        if (a == myBall)
        {
            CheckHitBall(b, opponentBall);
        }
        else if (b == myBall)
        {
            CheckHitBall(a, opponentBall);
        }
    }

    void CheckHitBall(BilliardBall hitBall, BilliardBall opponentBall)
    {
        if (hitBall.ballType == BallType.Target)
        {
            hitTargets.Add(hitBall);
        }

        if (hitBall == opponentBall)
        {
            hitOpponentBall = true;
        }
    }

    void FinishTurn()
    {
        bool allTargetsHit = hitTargets.Count >= 1;

        if (allTargetsHit)
        {
            AddScore(currentPlayer, 1);
        }

        if (hitOpponentBall)
        {
            AddScore(currentPlayer, -1);
        }

        if (player1Score >= 5 || player2Score >= 5)
        {
            gameOver = true;
        }
        else
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
        }

        isWaitingStop = false;
    }

    void AddScore(int player, int amount)
    {
        if (player == 1)
        {
            player1Score += amount;
            player1Score = Mathf.Max(0, player1Score);
        }
        else
        {
            player2Score += amount;
            player2Score = Mathf.Max(0, player2Score);
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.black;

        GUI.Label(new Rect(20, 20, 500, 50), "현재 턴 : " + currentPlayer + "P", style);
        GUI.Label(new Rect(20, 60, 500, 50), "1P 점수 : " + player1Score, style);
        GUI.Label(new Rect(20, 100, 500, 50), "2P 점수 : " + player2Score, style);

        if (isCharging)
        {
            GUI.Label(new Rect(20, 140, 500, 50), "힘 : " + currentPower.ToString("F1"), style);
        }

        if (gameOver)
        {
            GUI.Label(new Rect(20, 180, 500, 50), currentPlayer + "P 승리!", style);
        }
    }
    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }
}