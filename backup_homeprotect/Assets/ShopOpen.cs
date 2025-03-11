using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOpen : MonoBehaviour
{
    public GameObject scrollView; 
    public Button toggleButton;   
    void Start()
    {
        // ó���� ScrollView�� ��Ȱ��ȭ
        if (scrollView != null)
        {
            scrollView.SetActive(false);
        }

        // ��ư Ŭ�� �̺�Ʈ�� ToggleScrollView ����
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleScrollView);
        }
    }

    
        void ToggleScrollView()
        {
            if (scrollView != null)
            {
                scrollView.SetActive(!scrollView.activeSelf);
            }
        }
    
}
