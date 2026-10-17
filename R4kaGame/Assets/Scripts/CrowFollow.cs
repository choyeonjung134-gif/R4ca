
using UnityEngine;

public class CrowFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float stopDistance = 2f;
    public float tiltAngle = 25f;

    void Update()
    {
        if (player == null)
            return;

        // 알파카의 높이까지 포함한 방향
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= stopDistance || distance < 0.001f)
            return;

        // 알파카를 향해 위아래로도 이동
        direction.Normalize();

        transform.position +=
            direction * moveSpeed * Time.deltaTime;

        // 알파카를 바라보기
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // 앞으로 숙이는 자세
        Quaternion tiltRotation =
    Quaternion.Euler(tiltAngle, 0f, 0f);

        Quaternion modelCorrection =
            Quaternion.Euler(0f, 180f, 0f);

        transform.rotation =
            lookRotation * modelCorrection * tiltRotation;

       
    }
}
