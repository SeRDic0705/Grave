using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� �ʿ��� ���ӽ����̽�

public class OverManager : MonoBehaviour
{
    public void ReStartGame()
    {
        // Main ������ ��ȯ
        SceneManager.LoadScene("Start");
    }

}
