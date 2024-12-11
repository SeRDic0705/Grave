using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public Text waveText; // UI 텍스트를 연결합니다.

    private void Start()
    {
        if (waveText == null)
        {
            Debug.LogError("Wave Text is not assigned in the Inspector.");
            return;
        }

        UpdateWaveUI(); // 초기 UI 업데이트
    }

    private void Update()
    {
        UpdateWaveUI(); // 매 프레임 UI 업데이트
    }

    private void UpdateWaveUI()
    {
        if (GameManager.Instance != null)
        {
            waveText.text = "Wave: " + GameManager.wave.ToString();
        }
        else
        {
            waveText.text = "Wave: -"; // GameManager가 없는 경우 기본값
        }
    }
}
