using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ���� ����� ����ϱ� ���� �߰�

public class morningnight : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color day;
    public Color night;

    [Range(0.01f, 0.2f)]
    public float transitionTime;

    private bool isNight = false;
    public Button nightButton; //������ �ٲٴ� ��ư

    private void Awake()
    {
        float spritex = sr.sprite.bounds.size.x;
        float spritey = sr.sprite.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        transform.localScale = new Vector2(Mathf.Ceil(screenX / spritex), Mathf.Ceil(screenY / spritey));

        sr.color = day; // ó������ �� ����
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

    // ���� ����
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