using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChaser : MonoBehaviour
{
    public Transform player;
    public PlayerController playerController;

    public float rotateSpeed = 30f;     // 초당 회전속도
    public float viewAngle = 60f;       // 시야각
    public float viewDistance = 6f;     // 시야거리
    public float chaseSpeed = 5f;       // 추적속도
    public float parryCheckDistance = 1.8f; // 패링 판정 거리

    private bool isChasing = false;

    void Update()
    {
        if (player == null || playerController == null) return;

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            PatrolRotate();
            CheckPlayerInSight();
        }
    }

    void PatrolRotate()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    void CheckPlayerInSight()
    {
        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > viewDistance)
            return;

        Vector3 forward = transform.forward.normalized;
        Vector3 dirToPlayer = toPlayer.normalized;

        // 내적 직접 계산
        float dot = MyDot(forward, dirToPlayer);

        // 시야각 절반 기준의 cos값과 비교
        float cosValue = Mathf.Cos((viewAngle * 0.5f) * Mathf.Deg2Rad);

        if (dot >= cosValue)
        {
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // 플레이어를 향해 이동
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // 플레이어 쪽 보기
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                8f * Time.deltaTime
            );
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= parryCheckDistance)
        {
            CheckParry();
        }
    }

    void CheckParry()
    {
        // 플레이어 기준에서 적 방향
        Vector3 playerForward = player.forward.normalized;
        Vector3 playerToEnemy = (transform.position - player.position).normalized;

        // 외적 직접 계산
        Vector3 cross = MyCross(playerForward, playerToEnemy);

        // y값으로 좌/우 판정
        // y > 0 : 한쪽 / y < 0 : 반대쪽
        // 유니티 좌표계 기준으로 실제 게임에서 반대로 느껴질 수 있어서
        // 테스트 후 Q/E만 서로 바꾸면 됨
        bool enemyIsLeft = cross.y > 0f;
        bool enemyIsRight = cross.y < 0f;

        bool success = false;

        if (enemyIsLeft && playerController.isLeftParrying)
        {
            success = true;
        }
        else if (enemyIsRight && playerController.isRightParrying)
        {
            success = true;
        }

        if (success)
        {
            Destroy(gameObject);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    float MyDot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    Vector3 MyCross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }
}