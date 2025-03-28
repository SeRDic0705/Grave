using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public Button atkButton;
    public Button defButton;
    public Button hpButton;

    public Text atkText;
    public Text defText;
    public Text hpText;
    public Text pointText;
    public Text levelText;

    private Player player;

    private void Start()
    {
        player = Player.getInstance();

        if (player == null)
        {
            Debug.LogError("Player instance is not initialized! Ensure the Player script is properly attached.");
        }

        // 버튼 클릭 이벤트 등록
        atkButton.onClick.AddListener(() => IncreaseStat("atk"));
        defButton.onClick.AddListener(() => IncreaseStat("def"));
        hpButton.onClick.AddListener(() => IncreaseStat("hp"));
    }

    private void Update()
    {
        // 버튼 활성화 상태 업데이트
        bool hasPoints = player != null && player.point > 0;
        atkButton.interactable = hasPoints;
        defButton.interactable = hasPoints;
        hpButton.interactable = hasPoints;

        // 텍스트 업데이트
        if (player != null)
        {
            atkText.text = $"공격력: {player.atk}";
            defText.text = $"방어력: {player.def}";
            hpText.text = $"체력: {player.hp}";
            pointText.text = $"포인트: {player.point}";
            levelText.text = $"{player.level}";
        }

        // 숫자 1을 누르면 레벨업
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LevelUp();
        }
    }

    private void IncreaseStat(string statType)
    {
        if (player != null)
        {
            bool success = player.IncreaseStat(statType);
            if (!success)
            {
                Debug.LogWarning("Not enough points to increase stat.");
            }
        }
    }

    private void LevelUp()
    {
        if (player != null)
        {
            player.LevelUp();
        }
    }
}
