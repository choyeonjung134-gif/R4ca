using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;



public class PlayerShooter : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireCooldown = 0.2f;   // 연속 클릭 간격


    private float normalFireCoolDown = 0.2f;
    private float appleFireCoolDown = 0.01f;
    private float fireCoolDown = 0.2f;
    private Coroutine appleBoostCoroutine;

    public void ActivateAppleBoost()
    {
        if (appleBoostCoroutine != null)
        {
            StopCoroutine(appleBoostCoroutine);
        }

        appleBoostCoroutine = StartCoroutine(AppleBoostRoutine());
    }

    private IEnumerator AppleBoostRoutine()
    {
        fireCoolDown = appleFireCoolDown;

        yield return new WaitForSeconds(10f);

        fireCoolDown = normalFireCoolDown;
       
    }


    float lastFireTime = -999f;

    public void OnAttack(InputValue value)
    {
        if (Time.timeScale == 0f) return;   // 게임 오버면 무시
        if (Time.time < lastFireTime + fireCooldown) return;
        lastFireTime = Time.time;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Destroy(bullet, 3f); //3초 지나면 사라지기

        Vector3 shootDirection = playerCamera.transform.forward;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootDirection * bulletSpeed;
    }
}
