using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private static Player player = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
        	Destroy(this.gameObject);
        }

        player = Player.getInstance();
    }
    
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

}
