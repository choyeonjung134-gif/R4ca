using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class AlpacaSpit : MonoBehaviour
{
    [Header("필수")]
    public Transform mouthPoint;          // 알파카 입 위치의 빈 오브젝트

    [Header("선택")]
    public Transform aimPoint;            // 비워두면 Player 아래 FirePoint를 자동으로 찾음
    public GameObject spitPrefab;         // 비워두면 구체를 자동 생성
    public Color spitColor = new Color(0.7f, 0.9f, 1f);

    [Header("설정")]
    public float chargeTime = 1.5f;       // 다 커지는 데 걸리는 시간
    public float startScale = 0.1f;
    public float maxScale = 0.6f;
    public float launchSpeed = 15f;
    public float cooldown = 1f;
    public int damage = 1;

    [Header("포도 횟수")]
    public int spitCharges = 0;           // 지금 쓸 수 있는 횟수

    GameObject currentSpit;
    float chargeTimer;
    float nextAllowedTime;

    void Start()
    {
        if (aimPoint == null)
            aimPoint = transform.Find("FirePoint");
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

        // 커지는 동안엔 충돌 끄기
        Collider col = currentSpit.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        currentSpit.transform.localScale = Vector3.one * startScale;
        chargeTimer = 0f;
    }

    void Launch()
    {
        currentSpit.transform.SetParent(null);

        SpitProjectile proj = currentSpit.GetComponent<SpitProjectile>();
        if (proj == null) proj = currentSpit.AddComponent<SpitProjectile>();

        Vector3 dir = aimPoint != null ? aimPoint.forward : mouthPoint.forward;
        proj.Init(dir, launchSpeed, damage);

        currentSpit = null;
        nextAllowedTime = Time.time + cooldown;
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

        // 트리거 충돌이 되려면 Rigidbody가 하나 필요함
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

        // 맞은 대상에 TakeDamage(int)가 있으면 호출됨
        other.SendMessageUpwards("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }
}