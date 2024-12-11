using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownManager : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public Image cooldownImage;       // ��Ÿ�� �ִϸ��̼� �̹���
        public Image skillIcon;           // ��ų ������ �̹���
        public Text cooldownText;         // ��Ÿ�� �ؽ�Ʈ
        [HideInInspector] public float cooldownTime; // ��Ÿ�� �ð�
        [HideInInspector] public bool isOnCooldown;  // ��Ÿ�� ����
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
            SetSkillIconTransparency(skill.skillIcon, 1.0f); // �ʱ� ���´� ������
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

        // ��ų �������� �������ϰ� ����
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

        // ��Ÿ�� ���� �� �������� �ٽ� �������ϰ� ����
        SetSkillIconTransparency(skill.skillIcon, 1.0f);

        Debug.Log($"{skill.skillName} is ready to use!");
    }

    private void SetSkillIconTransparency(Image icon, float alpha)
    {
        Color color = icon.color;
        color.a = alpha; // ���� ���� ���� (0: ���� ����, 1: ���� ������)
        icon.color = color;
    }
}
