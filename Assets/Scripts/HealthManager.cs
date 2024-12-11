using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    private Player player;

    private void Start()
    {
        // Player 인스턴스 참조
        player = Player.getInstance();
        if (player == null)
        {
            Debug.LogError("Player instance is not found! Ensure the Player script is properly attached.");
        }
    }

    private void Update()
    {
        // Healthbar의 Fill Amount 업데이트
        if (player != null && healthBar != null)
        {
            healthBar.fillAmount = (float)player.currentHp / player.hp;
        }

        /*
        // 테스트 코드들
        // 숫자 1을 누르면 체력 감소
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10);
        }

        // 숫자 2를 누르면 체력 회복
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(10);
        }
        */
    }

    // Player의 Damage 함수 호출
    public void TakeDamage(int amount)
    {
        if (player != null)
        {
            player.Damage(amount);
        }
    }

    // Player의 Heal 함수 호출
    public void Heal(int amount)
    {
        if (player != null)
        {
            player.Heal(amount);
        }
    }
}
