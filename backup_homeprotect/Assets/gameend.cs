using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI 버튼을 사용하려면 필요함
using UnityEditor;     // Unity 에디터 종료를 위해 필요

public class gameend : MonoBehaviour
{
    // 버튼 클릭 시 실행할 메서드
    public void ExitGame()
    {
        if (Application.isEditor)  // Unity 에디터에서 실행 중인지 확인
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
