using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player player = null;
    public int hp;
    public float speed;
    public int atk;
    public int def;

    private Player()
    {
        hp = 100;
        speed = 3.0f;
        atk = 20;
        def = 5;
        Debug.Log("player created");
    }

    public static Player getInstance()
    {
        if(player == null)
        {
            player = new Player();
        }
        return player;
    }

}
