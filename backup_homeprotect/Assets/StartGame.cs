using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        // 마우스 클릭 또는 터치 입력 감지
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            StartGameScene();
        }
    }
    void StartGameScene()
    {
        // "morning"이라는 씬으로 이동 
        SceneManager.LoadScene("morning");
    }
}
