
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 까마귀에 맞으면 피격 처리
        CrowHealth crowHealth =
            collision.gameObject.GetComponentInParent<CrowHealth>();

        if (crowHealth != null)
        {
            crowHealth.TakeHit();
            Destroy(gameObject);
            return;
        }

        // 바닥에 닿으면 침 삭제
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
