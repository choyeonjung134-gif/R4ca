using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class AlpacaSpit : MonoBehaviour
{
    [Header("필수")]
    public Transform mouthPoint;          // 알파카 입 위치의 빈 오브젝트

    [Header("선택")]
    public Camera aimCamera;              // 비워두면 Main Camera를 사용
    public Transform aimPoint;            // 카메라가 없을 때만 쓰는 예비 방향
    public GameObject spitPrefab;         // 비워두면 구체를 자동 생성
    public Color spitColor = new Color(0.7f, 0.9f, 1f);

    [Header("설정")]
    public float chargeTime = 1.5f;
    public float startScale = 0.1f;
    public float maxScale = 0.6f;
    public float launchSpeed = 15f;
    public float cooldown = 1f;
    public int damage = 1;
    public float aimDistance = 60f;       // 화면 중앙에서 이 거리까지 조준점을 찾음

    [Header("포도 횟수")]
    public int spitCharges = 0;

    GameObject currentSpit;
    float chargeTimer;
    float nextAllowedTime;
    PlayerShooter shooter;

    void Start()
    {
        shooter = GetComponent<PlayerShooter>();

        if (aimCamera == null)
            aimCamera = Camera.main;

        if (aimPoint == null)
            aimPoint = transform.Find("FirePoint");
    }

    void OnDisable()
    {
        if (shooter != null) shooter.enabled = true;
    }

    public void AddCharge(int amount = 1)
    {
        spitCharges += amount;
        Debug.Log("침 스킬 횟수: " + spitCharges);
    }

    bool SpitKeyPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.digit3Key.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Alpha3);
#endif
    }

    void Update()
    {
        if (currentSpit == null)
        {
            if (SpitKeyPressed() && Time.time >= nextAllowedTime)
            {
                if (spitCharges > 0)
                    StartCharge();
                else
                    Debug.Log("포도가 없어서 침을 쓸 수 없음!");
            }
            return;
        }

        chargeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(chargeTimer / chargeTime);
        currentSpit.transform.localScale = Vector3.one * Mathf.Lerp(startScale, maxScale, t);

        if (t >= 1f)
            Launch();
    }

    void StartCharge()
    {
        if (mouthPoint == null)
        {
            Debug.LogWarning("Mouth Point가 비어 있음!");
            return;
        }

        spitCharges--;
        Debug.Log("침 스킬 남은 횟수: " + spitCharges);

        // 침이 커지는 동안 클릭 발사 막기
        if (shooter != null) shooter.enabled = false;

        if (spitPrefab != null)
        {
            currentSpit = Instantiate(spitPrefab, mouthPoint);
            currentSpit.transform.localPosition = Vector3.zero;
        }
        else
        {
            currentSpit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            currentSpit.transform.SetParent(mouthPoint);
            currentSpit.transform.localPosition = Vector3.zero;
            currentSpit.GetComponent<Renderer>().material.color = spitColor;
        }

        Collider col = currentSpit.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        currentSpit.transform.localScale = Vector3.one * startScale;
        chargeTimer = 0f;
    }

    // 카메라 화면 중앙이 바라보는 곳을 향하는 방향 계산 (위아래 포함)
    Vector3 GetAimDirection(Vector3 from)
    {
        if (aimCamera == null)
        {
            Debug.LogWarning("조준 카메라를 못 찾아서 FirePoint/MouthPoint 방향을 사용함");
            return aimPoint != null ? aimPoint.forward : mouthPoint.forward;
        }

        Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);
        Vector3 target = ray.GetPoint(aimDistance);

        RaycastHit[] hits = Physics.RaycastAll(ray, aimDistance, ~0, QueryTriggerInteraction.Ignore);
        float nearest = float.MaxValue;

        foreach (RaycastHit h in hits)
        {
            // 플레이어 자신(알파카)은 무시
            if (h.transform.IsChildOf(transform)) continue;

            if (h.distance < nearest)
            {
                nearest = h.distance;
                target = h.point;
            }
        }

        Vector3 dir = target - from;

        // 조준점이 너무 가까우면 카메라 방향을 그대로 사용
        if (dir.sqrMagnitude < 1f)
            return aimCamera.transform.forward;

        return dir.normalized;
    }

    void Launch()
    {
        currentSpit.transform.SetParent(null);

        Vector3 dir = GetAimDirection(currentSpit.transform.position);

        SpitProjectile proj = currentSpit.GetComponent<SpitProjectile>();
        if (proj == null) proj = currentSpit.AddComponent<SpitProjectile>();

        // 디버그: 발사 방향 확인용
        Debug.Log("침 발사 방향: " + dir);
        Debug.DrawRay(currentSpit.transform.position, dir * 10f, Color.red, 3f);

        proj.Init(dir, launchSpeed, damage);

        currentSpit = null;
        nextAllowedTime = Time.time + cooldown;

        // 발사 후 클릭 발사 다시 허용
        if (shooter != null) shooter.enabled = true;
    }
}

public class SpitProjectile : MonoBehaviour
{
    Vector3 dir;
    float speed;
    int damage;
    bool launched;

    public void Init(Vector3 direction, float spd, int dmg)
    {
        dir = direction.normalized;
        speed = spd;
        damage = dmg;
        launched = true;

        Collider col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (!launched) return;
        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!launched) return;
        if (other.CompareTag("Player")) return;

        other.SendMessageUpwards("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }
}