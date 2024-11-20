using UnityEngine;
using UnityEngine.UI; // UI ��Ҹ� �����ϱ� ���� �ʿ�

public class HealthManager : MonoBehaviour
{
    public Slider healthBar; // ü�¹� UI
    public float maxHealth = 100f; // �ִ� ü��
    private float currentHealth; // ���� ü��

    void Start()
    {
        currentHealth = maxHealth; // ü���� �ִ밪���� �ʱ�ȭ
        healthBar.maxValue = maxHealth; // ü�¹� �ִ밪 ����
        healthBar.value = currentHealth; // ���� ü�� ����
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ��������ŭ ü�� ����
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü�� ������ ����
        healthBar.value = currentHealth; // ü�¹� ������Ʈ
    }

    public void Heal(float amount)
    {
        currentHealth += amount; // ȸ������ŭ ü�� ����
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü�� ������ ����
        healthBar.value = currentHealth; // ü�¹� ������Ʈ
    }
}
