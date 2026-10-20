
using UnityEngine;

public class StrawberryPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Character Controller가 있는 캐릭터인지 확인
        CharacterController player =
            other.GetComponentInParent<CharacterController>();

        if (player != null)
        {
            Debug.Log("딸기 먹음!");

            Destroy(gameObject);
        }
    }
}

