
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 뱀을 맞혔는지 확인
        SnakeHealth snake =
            collision.gameObject.GetComponentInParent<SnakeHealth>();

        if (snake != null)
        {
            snake.TakeHit();
            Destroy(gameObject);
            return;
        }

        // 까마귀를 맞혔는지 확인
        CrowHealth crow =
            collision.gameObject.GetComponentInParent<CrowHealth>();

        if (crow != null)
        {
            crow.TakeHit();
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
