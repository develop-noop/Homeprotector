using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        // ���콺 Ŭ�� �Ǵ� ��ġ �Է� ����
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            StartGameScene();
        }
    }
    void StartGameScene()
    {
        // "morning"�̶�� ������ �̵� 
        SceneManager.LoadScene("morning");
    }
}
