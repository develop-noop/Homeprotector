using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Trans_Btn : MonoBehaviour
{
    public Button myButton; //��ư ����
    public Image buttonImage; // ��ư �̹���(UI �����)
    public Sprite stateOneSprite; //ù ��° ���� �̹���
    public Sprite stateTwoSprite; //�� ��° ���� �̹���

    private enum ButtonState { StateOne, StateTwo };
    private ButtonState currentSate = ButtonState.StateOne; //�ʱ� ���� ����

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick); //��ư Ŭ�� �̺�Ʈ ���
        buttonImage.sprite = stateOneSprite; //�ʱ� ��ư ����
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        if (currentSate == ButtonState.StateOne)
        {
            PerformActionOne();
            currentSate = ButtonState.StateTwo;
            buttonImage.sprite = stateTwoSprite; //�̹��� ����

        }
        else
        {
            PerformActionTwo();
            currentSate = ButtonState.StateOne;
            buttonImage.sprite = stateOneSprite; //�̹��� ����
        }
    }
    void PerformActionOne()
        {
        Debug.Log("ù ��° ���¿��� ���� ����!");
            // ù ��° ���¿��� ������ ���
        Time.timeScale = 0f; //���� �Ͻ�����
        Debug.Log("������ �Ͻ�����");
        }
    void PerformActionTwo()
    {
        Debug.Log("�� ��° ���¿��� ���� ����!");
        
        Time.timeScale = 1f; //���� �Ͻ�����
        Debug.Log("������ �ٽ� ����");
    }
}
