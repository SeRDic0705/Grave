using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    private Player player;

    private void Start()
    {
        // Player �ν��Ͻ� ����
        player = Player.getInstance();
        if (player == null)
        {
            Debug.LogError("Player instance is not found! Ensure the Player script is properly attached.");
        }
    }

    private void Update()
    {
        // Healthbar�� Fill Amount ������Ʈ
        if (player != null && healthBar != null)
        {
            healthBar.fillAmount = (float)player.currentHp / player.hp;
        }

        /*
        // �׽�Ʈ �ڵ��
        // ���� 1�� ������ ü�� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10);
        }

        // ���� 2�� ������ ü�� ȸ��
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(10);
        }
        */
    }

    // Player�� Damage �Լ� ȣ��
    public void TakeDamage(int amount)
    {
        if (player != null)
        {
            player.Damage(amount);
        }
    }

    // Player�� Heal �Լ� ȣ��
    public void Heal(int amount)
    {
        if (player != null)
        {
            player.Heal(amount);
        }
    }
}
