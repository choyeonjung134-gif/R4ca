using UnityEngine;

public class ApplePowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooter shooter = other.GetComponent<PlayerShooter>();

            if (shooter != null)
            {
                shooter.ActivateAppleBoost();
                Destroy(gameObject);
            }
        }
    }
}