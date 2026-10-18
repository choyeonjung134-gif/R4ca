
using System.Collections;
using UnityEngine;

public class BerryShield : MonoBehaviour
{
    [Header("보호막 설정")]
    public GameObject shieldObject;
    public float duration = 10f;
    public int maxHits = 3;

    private int remainingHits;
    private bool isActive = false;
    private Coroutine shieldCoroutine;

    void Start()
    {
        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    void Update()
    {
        // 2번 키를 누르면 보호막 발동
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateShield();
        }
    }

    public void ActivateShield()
    {
        if (shieldObject == null)
        {
            Debug.LogWarning("보호막 오브젝트를 연결해 주세요.");
            return;
        }

        // 이미 활성화된 경우 다시 시작
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);

        remainingHits = maxHits;
        isActive = true;

        shieldObject.SetActive(true);

        shieldCoroutine = StartCoroutine(ShieldTimer());

        Debug.Log("딸기 보호막 발동!");
    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(duration);
        DeactivateShield();
    }

    // 공격 스크립트에서 호출할 함수
    public bool TryBlockHit()
    {
        if (!isActive)
            return false;

        remainingHits--;

        Debug.Log("보호막 방어! 남은 횟수: " + remainingHits);

        if (remainingHits <= 0)
            DeactivateShield();

        return true;
    }

    void DeactivateShield()
    {
        isActive = false;

        if (shieldObject != null)
            shieldObject.SetActive(false);

        shieldCoroutine = null;

        Debug.Log("보호막 종료");
    }
}

