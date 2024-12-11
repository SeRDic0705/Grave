using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환에 필요한 네임스페이스

public class TheGameStart : MonoBehaviour
{
    public void StartGame()
    {
        // Main 씬으로 전환
        SceneManager.LoadScene("Main");
    }

}
