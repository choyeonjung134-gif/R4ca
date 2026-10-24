using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public float detectRange = 15f;
    public float attackRange = 2.5f;
    public int damage = 1;
    public float attackCooldown = 1f;

    Transform player;
    BerryShield playerShield;
    Health playerHealth;
    CrowFollow crowFollow;
    float lastAttackTime = -999f;

    void Start()
    {
        crowFollow = GetComponent<CrowFollow>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p == null)
        {
            Debug.LogWarning("Player 태그 오브젝트를 찾을 수 없음!");
            return;
        }

        player = p.transform;
        playerShield = p.GetComponent<BerryShield>();
        playerHealth = p.GetComponent<Health>();

        if (playerHealth == null)
            Debug.LogWarning("Player에 Health 스크립트가 없음!");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > detectRange) return;

        // 까마귀가 멈추는 거리보다 항상 조금 더 멀리까지 공격 가능하게 함
        float range = attackRange;
        if (crowFollow != null)
            range = Mathf.Max(attackRange, crowFollow.stopDistance + 0.5f);

        if (distance > range) return;
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        bool blocked = playerShield != null && playerShield.TryBlockHit();
        if (blocked)
        {
            Debug.Log("까마귀 공격을 보호막이 막았습니다!");
            return;
        }

        if (playerHealth != null)
        {
            Debug.Log("까마귀가 데미지를 줌!");
            playerHealth.TakeDamage(damage);
        }
    }
}