
using UnityEngine;

public class OasisHeal : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health == null)
        {
            health = other.GetComponentInParent<Health>();
        }

        if (health == null || !health.CompareTag("Player"))
            return;

        health.Heal(healAmount);
    }
}
