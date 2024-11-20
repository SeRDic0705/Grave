using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public TextMeshProUGUI goldText; // 골드 텍스트 UI 연결
    private int currentGold = 0; // 현재 골드
    public float goldIncrementInterval = 1f; // 골드 증가 간격 (초)

    void Start()
    {
        UpdateGoldText(); // UI 초기화
        // 일정 주기로 골드 증가 시작
        InvokeRepeating(nameof(AutoAddGold), goldIncrementInterval, goldIncrementInterval);
    }

    // 골드를 자동으로 추가
    private void AutoAddGold()
    {
        AddGold(1); // 골드를 1씩 추가
    }

    public void AddGold(int amount)
    {
        currentGold += amount; // 골드 추가
        UpdateGoldText(); // UI 업데이트
    }

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + currentGold.ToString(); // UI 텍스트 업데이트
    }
}
