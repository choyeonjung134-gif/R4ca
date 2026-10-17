using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHp = 3;
    public int currentHp;
    public Slider hpSlider;      // �÷��̾ ����

    void Start()
    {
        currentHp = maxHp;
        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (currentHp <= 0) return;   // �̹� �׾����� ����

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
            hpSlider.value = (float)currentHp / maxHp;
    }

    void Die()
    {
        if (CompareTag("Player"))
        {
            Debug.Log("���� ����");
            Time.timeScale = 0f;      // ���� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
