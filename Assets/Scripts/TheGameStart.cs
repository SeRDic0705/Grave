using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� �ʿ��� ���ӽ����̽�

public class TheGameStart : MonoBehaviour
{
    public void StartGame()
    {
        // Main ������ ��ȯ
        SceneManager.LoadScene("Main");
    }

}
