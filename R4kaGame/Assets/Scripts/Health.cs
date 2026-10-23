using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHp = 3;
    public int currentHp;
    public Slider hpSlider;
    public GameObject gameOverPanel;// �÷��̾ ����
    public static int killCount;    // 모든 Health 가 같이 쓰는 값 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (currentHp <= 0) return;

        currentHp -= damage;
        Debug.Log(name + " HP: " + currentHp);
        UpdateBar();

        if (currentHp <= 0) Die();
    }

    // ㅡㅡ 오아시스 회복 기능 추가

    public void Heal(int amount)
    {
        if (currentHp <= 0) return;

        currentHp += amount;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        Debug.Log(name + " HP 회복: " + currentHp);
        UpdateBar();
    }

    // ㅡㅡ

    void UpdateBar()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;
        }
    }

    void Die()
    {
        if (CompareTag("Player"))
        {
            Debug.Log("플레이어 사망");

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            // 마우스 커서를 보이게 하고 잠금 해제
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 게임 일시정지
            Time.timeScale = 0f;
        }
    }
}
