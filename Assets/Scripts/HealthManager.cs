using UnityEngine;
using UnityEngine.UI; // UI 요소를 제어하기 위해 필요

public class HealthManager : MonoBehaviour
{
    public Slider healthBar; // 체력바 UI
    public float maxHealth = 100f; // 최대 체력
    private float currentHealth; // 현재 체력

    void Start()
    {
        currentHealth = maxHealth; // 체력을 최대값으로 초기화
        healthBar.maxValue = maxHealth; // 체력바 최대값 설정
        healthBar.value = currentHealth; // 현재 체력 설정
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // 데미지만큼 체력 감소
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위를 제한
        healthBar.value = currentHealth; // 체력바 업데이트
    }

    public void Heal(float amount)
    {
        currentHealth += amount; // 회복량만큼 체력 증가
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위를 제한
        healthBar.value = currentHealth; // 체력바 업데이트
    }
}
