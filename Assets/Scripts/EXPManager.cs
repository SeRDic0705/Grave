using UnityEngine;
using UnityEngine.UI;

public class EXPManager : MonoBehaviour
{
    public Image EXPBar;
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
        if (player != null && EXPBar != null)
        {
            EXPBar.fillAmount = (float)player.currentEXP / player.EXP;
        }

        /*
        // 숫자 9을 누르면 경험치 증가 테스트 코드
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GainEXP(40);
        }
        */
    }

    // Player의 Damage 함수 호출
    public void GainEXP(int amount)
    {
        if (player != null)
        {
            player.GainEXP(amount);
        }
    }

    
}
