using UnityEngine;

public class Player
{
    private static Player player = null;

    public int hp;
    public int currentHp;
    public float speed;
    public int atk;
    public int def;
    public int level;
    public int point;
    public float skill1Cooldown = 5f;
    public float skill2Cooldown = 7f;
    public float skill3Cooldown = 10f;

    private Player()
    {
        // �⺻ �Ӽ� �ʱ�ȭ
        hp = 100;
        currentHp = 100;
        speed = 3.0f;
        atk = 20;
        def = 5;
        level = 1;
        point = 0;

        Debug.Log("Player initialized");
    }

    public static Player getInstance()
    {
        if (player == null)
        {
            player = new Player();
        }
        return player;
    }

    public void Damage(int amount)
    {
        currentHp -= amount;
        if (currentHp < 0)
            currentHp = 0;
        Debug.Log($"Player took {amount} damage. Current HP: {currentHp}");
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > hp)
            currentHp = hp;
        Debug.Log($"Player healed {amount}. Current HP: {currentHp}");
    }

    public void LevelUp()
    {
        level += 1;
        point += 1;
        Debug.Log($"Level Up! Current Level: {level}, Points: {point}");
    }

    public bool IncreaseStat(string statType)
    {
        if (point <= 0) return false;

        switch (statType.ToLower())
        {
            case "atk":
                atk += 1;
                break;
            case "def":
                def += 1;
                break;
            case "hp":
                hp += 10;
                currentHp += 10;
                break;
            default:
                Debug.LogError("Invalid stat type.");
                return false;
        }

        point -= 1;
        Debug.Log($"{statType} increased! Remaining Points: {point}");
        return true;
    }

}
