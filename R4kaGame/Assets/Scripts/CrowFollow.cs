
using UnityEngine;

public class CrowFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float stopDistance = 2f;
    public float tiltAngle = 25f;

    // 딸기 스크립트 능력

    public float attackInterval = 1f;

    private float nextAttackTime = 0f;
    private BerryShield playerShield;

    //딸기 스크립트 능력

    void Update()
    {
        if (player == null)
            return;
        // 딸기 스크립트 능력
        if (playerShield == null)
            playerShield = player.GetComponent<BerryShield>();
        // 딸기 스크립트 능력

        // 알파카의 높이까지 포함한 방향
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= stopDistance)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackInterval;

                if (playerShield != null)
                {
                    bool blocked = playerShield.TryBlockHit();

                    if (blocked)
                        Debug.Log("까마귀 공격을 보호막이 막았습니다!");
                    else
                        Debug.Log("까마귀가 공격했습니다!");
                }
            }

            return;
        }

        if (distance < 0.001f)
            return;

        // 알파카를 향해 위아래로도 이동
        direction.Normalize();

        transform.position +=
            direction * moveSpeed * Time.deltaTime;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, 0f, 0f);

        transform.rotation = lookRotation * tiltRotation; ;


    }
}
