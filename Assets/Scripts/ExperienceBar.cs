using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image experienceFill; // ExperienceBarFill 연결
    private float currentExperience = 0f;
    private float maxExperience = 100f;

    void Update()
    {
        // 경험치 바 업데이트 (임시로 테스트용 경험치 증가 코드)
        if (currentExperience < maxExperience)
        {
            currentExperience += Time.deltaTime * 10; // 경험치 증가 속도
            UpdateExperienceBar();
        }
    }

    public void UpdateExperienceBar()
    {
        float fillAmount = currentExperience / maxExperience;
        experienceFill.fillAmount = fillAmount;
    }

    public void AddExperience(float amount)
    {
        currentExperience = Mathf.Clamp(currentExperience + amount, 0, maxExperience);
        UpdateExperienceBar();
    }
}
