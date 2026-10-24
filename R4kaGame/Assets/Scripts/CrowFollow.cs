using UnityEngine;

public class CrowFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float stopDistance = 2f;
    public float tiltAngle = 25f;

    [Tooltip("모델이 뒤를 보고 있으면 180")]
    public float modelYawOffset = 180f;

    public float rotateSpeed = 10f;

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        RotateTowards(direction);

        if (distance <= stopDistance) return;

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    void RotateTowards(Vector3 direction)
    {
        Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
        if (flatDir.sqrMagnitude < 0.0001f) return;

        Quaternion lookRotation = Quaternion.LookRotation(flatDir.normalized);
        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
        Quaternion modelFix = Quaternion.Euler(0f, modelYawOffset, 0f);

        Quaternion targetRotation = lookRotation * tiltRotation * modelFix;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }
}