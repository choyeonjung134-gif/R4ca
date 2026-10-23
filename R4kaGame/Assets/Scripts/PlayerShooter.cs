
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShooter : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float bulletSpeed = 20f;
    public float fireCooldown = 0.2f;

    private float normalFireCooldown = 0.2f;
    private float appleFireCooldown = 0.08f;
    private float currentFireCooldown = 0.2f;

    private Coroutine appleBoostCoroutine;
    private float lastFireTime = -999f;

    private bool hasApple = false;

    public void ActivateAppleBoost()
    {
        hasApple = true;
    }

    private IEnumerator AppleBoostRoutine()
    {
        currentFireCooldown = appleFireCooldown;

        yield return new WaitForSeconds(10f);

        currentFireCooldown = normalFireCooldown;
        appleBoostCoroutine = null;
    }



    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        // 사과를 먹은 뒤 숫자 1을 누르면 능력 발동
        if (hasApple &&
            Keyboard.current != null &&
            Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            hasApple = false;
            StartCoroutine(AppleBoostRoutine());
        }

        // 마우스 왼쪽 버튼을 누르고 있을 때 발사
        if (Mouse.current == null ||
            !Mouse.current.leftButton.isPressed)
            return;

        if (Time.time < lastFireTime + currentFireCooldown)
            return;

        Shoot();
    }



    private void Shoot()
    {
        if (playerCamera == null ||
            bulletPrefab == null ||
            firePoint == null)
            return;

        lastFireTime = Time.time;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Destroy(bullet, 3f);

        Vector3 shootDirection = playerCamera.transform.forward;


        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearVelocity = shootDirection * bulletSpeed;
        }

    }
}
