using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CloseScrollview : MonoBehaviour
{
    public GameObject scrollView; // ��ũ�� �� ������Ʈ
    void Start()
    {
        // ��ư�� ���� ���� ������Ʈ�� ���� ��� �ڵ����� ������
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(CloseView);
        }
    }

    public void CloseView()
    {
        if (scrollView != null)
        {
            scrollView.SetActive(false);
        }
    }
}
