using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI ��ư�� ����Ϸ��� �ʿ���
using UnityEditor;     // Unity ������ ���Ḧ ���� �ʿ�

public class gameend : MonoBehaviour
{
    // ��ư Ŭ�� �� ������ �޼���
    public void ExitGame()
    {
        if (Application.isEditor)  // Unity �����Ϳ��� ���� ������ Ȯ��
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
