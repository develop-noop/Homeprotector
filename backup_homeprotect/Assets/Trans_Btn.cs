using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Trans_Btn : MonoBehaviour
{
    public Button myButton; //버튼 참조
    public Image buttonImage; // 버튼 이미지(UI 변경용)
    public Sprite stateOneSprite; //첫 번째 상태 이미지
    public Sprite stateTwoSprite; //두 번째 상태 이미지

    private enum ButtonState { StateOne, StateTwo };
    private ButtonState currentSate = ButtonState.StateOne; //초기 상태 설정

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick); //버튼 클릭 이벤트 등록
        buttonImage.sprite = stateOneSprite; //초기 버튼 상태
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        if (currentSate == ButtonState.StateOne)
        {
            PerformActionOne();
            currentSate = ButtonState.StateTwo;
            buttonImage.sprite = stateTwoSprite; //이미지 변경

        }
        else
        {
            PerformActionTwo();
            currentSate = ButtonState.StateOne;
            buttonImage.sprite = stateOneSprite; //이미지 변경
        }
    }
    void PerformActionOne()
        {
        Debug.Log("첫 번째 상태에서 동작 실행!");
            // 첫 번째 상태에서 실행할 기능
        Time.timeScale = 0f; //게임 일시정지
        Debug.Log("게임이 일시정지");
        }
    void PerformActionTwo()
    {
        Debug.Log("두 번째 상태에서 동작 실행!");
        
        Time.timeScale = 1f; //게임 일시정지
        Debug.Log("게임이 다시 시작");
    }
}
