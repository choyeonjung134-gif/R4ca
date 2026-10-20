
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
    private bool hasStrawberry = false;
    private Coroutine shieldCoroutine;

    void Start()
    {
        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    void Update()
    {
        // 딸기를 먹은 뒤에만 2번 키 사용 가능
        if (hasStrawberry && Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasStrawberry = false; // 딸기 1개 사용
            ActivateShield();
        }
    }

    // 딸기를 먹었을 때 호출
    public void CollectStrawberry()
    {
        hasStrawberry = true;
        Debug.Log("딸기 획득! 2번 키를 눌러 보호막을 사용하세요.");
    }

    public void ActivateShield()
    {
        if (shieldObject == null)
        {
            Debug.LogWarning("Shield Object에 보호막을 연결해 주세요.");
            return;
        }

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

    // 까마귀 공격 스크립트에서 호출
    public bool TryBlockHit()
    {
        if (!isActive)
            return false;

        remainingHits--;

        Debug.Log("보호막 방어! 남은 방어 횟수: " + remainingHits);

        if (remainingHits <= 0)
            DeactivateShield();

        return true;
    }

    void DeactivateShield()
    {
        isActive = false;

        if (shieldObject != null)
            shieldObject.SetActive(false);

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            shieldCoroutine = null;
        }

        Debug.Log("보호막 종료");
    }
}
