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

        // ��ư Ŭ�� �̺�Ʈ ���
        atkButton.onClick.AddListener(() => IncreaseStat("atk"));
        defButton.onClick.AddListener(() => IncreaseStat("def"));
        hpButton.onClick.AddListener(() => IncreaseStat("hp"));
    }

    private void Update()
    {
        // ��ư Ȱ��ȭ ���� ������Ʈ
        bool hasPoints = player != null && player.point > 0;
        atkButton.interactable = hasPoints;
        defButton.interactable = hasPoints;
        hpButton.interactable = hasPoints;

        // �ؽ�Ʈ ������Ʈ
        if (player != null)
        {
            atkText.text = $"���ݷ�: {player.atk}";
            defText.text = $"����: {player.def}";
            hpText.text = $"ü��: {player.hp}";
            pointText.text = $"����Ʈ: {player.point}";
            levelText.text = $"{player.level}";
        }

        /*
        // ���� 3�� ������ ������ �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LevelUp();
        }
        */
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
