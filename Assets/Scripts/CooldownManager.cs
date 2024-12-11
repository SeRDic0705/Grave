using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownManager : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public Image cooldownImage;       // 쿨타임 애니메이션 이미지
        public Image skillIcon;           // 스킬 아이콘 이미지
        public Text cooldownText;         // 쿨타임 텍스트
        [HideInInspector] public float cooldownTime; // 쿨타임 시간
        [HideInInspector] public bool isOnCooldown;  // 쿨타임 상태
    }

    public Skill[] skills;
    private Player player;

    private void Start()
    {
        player = Player.getInstance();
        if (player == null)
        {
            Debug.LogError("Player instance not found! Make sure Player is initialized.");
            return;
        }

        skills[0].cooldownTime = player.skill1Cooldown;
        skills[1].cooldownTime = player.skill2Cooldown;
        skills[2].cooldownTime = player.skill3Cooldown;

        foreach (Skill skill in skills)
        {
            skill.cooldownImage.fillAmount = 0f;
            skill.cooldownText.text = "";
            SetSkillIconTransparency(skill.skillIcon, 1.0f); // 초기 상태는 불투명
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0); // Skill1
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1); // Skill2
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2); // Skill3
        }
    }

    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Length)
        {
            Debug.LogError("Invalid skill index!");
            return;
        }

        Skill skill = skills[skillIndex];
        if (skill.isOnCooldown)
        {
            Debug.Log($"{skill.skillName} is on cooldown!");
            return;
        }

        StartCoroutine(CooldownRoutine(skill));
        Debug.Log($"{skill.skillName} used!");
    }

    private IEnumerator CooldownRoutine(Skill skill)
    {
        skill.isOnCooldown = true;
        float cooldownRemaining = skill.cooldownTime;

        // 스킬 아이콘을 반투명하게 설정
        SetSkillIconTransparency(skill.skillIcon, 0.5f);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            skill.cooldownImage.fillAmount = cooldownRemaining / skill.cooldownTime;

            if (cooldownRemaining > 1f)
            {
                skill.cooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();
            }
            else
            {
                skill.cooldownText.text = cooldownRemaining.ToString("F1");
            }

            yield return null;
        }

        skill.cooldownImage.fillAmount = 0f;
        skill.cooldownText.text = "";
        skill.isOnCooldown = false;

        // 쿨타임 종료 후 아이콘을 다시 불투명하게 설정
        SetSkillIconTransparency(skill.skillIcon, 1.0f);

        Debug.Log($"{skill.skillName} is ready to use!");
    }

    private void SetSkillIconTransparency(Image icon, float alpha)
    {
        Color color = icon.color;
        color.a = alpha; // 알파 값을 설정 (0: 완전 투명, 1: 완전 불투명)
        icon.color = color;
    }
}
