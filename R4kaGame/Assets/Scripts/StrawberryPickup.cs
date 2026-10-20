using UnityEngine;

public class StrawberryPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        BerryShield shield = other.GetComponent<BerryShield>();

        if (shield == null)
            shield = other.GetComponentInParent<BerryShield>();

        if (shield == null)
        {
            Debug.LogWarning("Player에서 BerryShield를 찾을 수 없습니다.");
            return;
        }

        shield.CollectStrawberry();

        Debug.Log("딸기 획득!");

        Destroy(gameObject);
    }
}
