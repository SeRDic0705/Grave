using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환에 필요한 네임스페이스

public class OverManager : MonoBehaviour
{
    public void ReStartGame()
    {
        // Main 씬으로 전환
        SceneManager.LoadScene("Start");
    }

}
