using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 추가

public class morningnight : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color day;
    public Color night;

    [Range(0.01f, 0.2f)]
    public float transitionTime;

    private bool isNight = false;
    public Button nightButton; //밤으로 바꾸는 버튼

    private void Awake()
    {
        float spritex = sr.sprite.bounds.size.x;
        float spritey = sr.sprite.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        transform.localScale = new Vector2(Mathf.Ceil(screenX / spritex), Mathf.Ceil(screenY / spritey));

        sr.color = day; // 처음에는 낮 상태
        nightButton.onClick.AddListener(ChangeNight);
    }

    public void ChangeNight()
    {
        if (!isNight)
        {
            StartCoroutine(SwapColor(sr.color, night));
            isNight = true;
        }
    }

    // 색깔 변경
    IEnumerator SwapColor(Color start, Color end)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            sr.color = Color.Lerp(start, end, t);
            yield return null;
        }
    }
}