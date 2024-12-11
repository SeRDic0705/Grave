using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public Text waveText; // UI �ؽ�Ʈ�� �����մϴ�.

    private void Start()
    {
        if (waveText == null)
        {
            Debug.LogError("Wave Text is not assigned in the Inspector.");
            return;
        }

        UpdateWaveUI(); // �ʱ� UI ������Ʈ
    }

    private void Update()
    {
        UpdateWaveUI(); // �� ������ UI ������Ʈ
    }

    private void UpdateWaveUI()
    {
        if (GameManager.Instance != null)
        {
            waveText.text = "Wave: " + GameManager.wave.ToString();
        }
        else
        {
            waveText.text = "Wave: -"; // GameManager�� ���� ��� �⺻��
        }
    }
}
