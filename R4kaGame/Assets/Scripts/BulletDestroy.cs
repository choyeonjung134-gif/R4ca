using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
  private  void OnCollisionEnter(Collision collision)
    {
        //바닥에 닿으면 침 삭제
        if (collision.gameObject.CompareTag("Ground"))
            {
            Destroy(gameObject);
        }
    }
}
