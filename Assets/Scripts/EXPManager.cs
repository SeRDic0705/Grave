using UnityEngine;
using UnityEngine.UI;

public class EXPManager : MonoBehaviour
{
    public Image EXPBar;
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
        if (player != null && EXPBar != null)
        {
            EXPBar.fillAmount = (float)player.currentEXP / player.EXP;
        }

        /*
        // ���� 9�� ������ ����ġ ���� �׽�Ʈ �ڵ�
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GainEXP(40);
        }
        */
    }

    // Player�� Damage �Լ� ȣ��
    public void GainEXP(int amount)
    {
        if (player != null)
        {
            player.GainEXP(amount);
        }
    }

    
}
