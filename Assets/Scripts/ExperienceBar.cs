using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image experienceFill; // ExperienceBarFill 연결
    public Text levelText; // 레벨 텍스트를 UI와 연결
    private float currentExperience = 0f;
    private float maxExperience = 100f;
    private int currentLevel = 1; // 현재 레벨

    void Start()
    {
        UpdateLevelText(); // 레벨 텍스트 초기화
    }

    void Update()
    {
        // 경험치 바 업데이트 (임시로 테스트용 경험치 증가 코드)
        if (currentExperience < maxExperience)
        {
            currentExperience += Time.deltaTime * 10; // 경험치 증가 속도 (테스트용)
            UpdateExperienceBar();
        }
        else
        {
            LevelUp(); // 경험치가 최대치에 도달하면 레벨 업
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
        currentExperience -= maxExperience; // 초과된 경험치 유지
        currentLevel++; // 레벨 증가
        UpdateLevelText();
        UpdateExperienceBar();
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level: " + currentLevel.ToString(); // 레벨 텍스트 업데이트
    }
}
