using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image experienceFill; // ExperienceBarFill ����
    public Text levelText; // ���� �ؽ�Ʈ�� UI�� ����
    private float currentExperience = 0f;
    private float maxExperience = 100f;
    private int currentLevel = 1; // ���� ����

    void Start()
    {
        UpdateLevelText(); // ���� �ؽ�Ʈ �ʱ�ȭ
    }

    void Update()
    {
        // ����ġ �� ������Ʈ (�ӽ÷� �׽�Ʈ�� ����ġ ���� �ڵ�)
        if (currentExperience < maxExperience)
        {
            currentExperience += Time.deltaTime * 10; // ����ġ ���� �ӵ� (�׽�Ʈ��)
            UpdateExperienceBar();
        }
        else
        {
            LevelUp(); // ����ġ�� �ִ�ġ�� �����ϸ� ���� ��
        }
    }

    public void UpdateExperienceBar()
    {
        float fillAmount = currentExperience / maxExperience;
        experienceFill.fillAmount = fillAmount;
    }

    public void AddExperience(float amount)
    {
        currentExperience += amount;
        if (currentExperience >= maxExperience)
        {
            LevelUp();
        }
        else
        {
            UpdateExperienceBar();
        }
    }

    private void LevelUp()
    {
        currentExperience -= maxExperience; // �ʰ��� ����ġ ����
        currentLevel++; // ���� ����
        UpdateLevelText();
        UpdateExperienceBar();
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level: " + currentLevel.ToString(); // ���� �ؽ�Ʈ ������Ʈ
    }
}
