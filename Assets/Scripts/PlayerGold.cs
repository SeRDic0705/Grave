using TMPro; // TextMeshPro ���ӽ����̽� �߰�
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public TextMeshProUGUI goldText; // ��� �ؽ�Ʈ UI ����
    private int currentGold = 0; // ���� ���
    public float goldIncrementInterval = 1f; // ��� ���� ���� (��)

    void Start()
    {
        UpdateGoldText(); // UI �ʱ�ȭ
        // ���� �ֱ�� ��� ���� ����
        InvokeRepeating(nameof(AutoAddGold), goldIncrementInterval, goldIncrementInterval);
    }

    // ��带 �ڵ����� �߰�
    private void AutoAddGold()
    {
        AddGold(1); // ��带 1�� �߰�
    }

    public void AddGold(int amount)
    {
        currentGold += amount; // ��� �߰�
        UpdateGoldText(); // UI ������Ʈ
    }

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + currentGold.ToString(); // UI �ؽ�Ʈ ������Ʈ
    }
}
