using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �� ���� ����

public class FadeBlinkText : MonoBehaviour
{
    public Text textComponent; // UI �ؽ�Ʈ
    public float fadeDuration = 1f; // ���̵� ��/�ƿ� ���� �ð�
    private bool isSceneActive = true; // ���� �����Ǵ� ���� true
    void Start()
    {
        StartCoroutine(FadeBlink());
    }
    IEnumerator FadeBlink() // ���� �����Ǵ� ���� ����
    {
        while (isSceneActive)
        {
            // ���� �������� (Alpha 1 -> 0)
            yield return StartCoroutine(FadeText(1, 0, fadeDuration));
            // ���� �������� (Alpha 0 -> 1)
            yield return StartCoroutine(FadeText(0, 1, fadeDuration));
        }
    }
    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        Color textColor = textComponent.color;

        while (time < duration) //������ �ð�(duration) ���� Alpha �� ����
        {
            time += Time.deltaTime; //�� �������� �������� �ð�(Time.deltaTime)�� ���� time �� ���� ��Ŵ
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration); //�ε巴�� ��ȭ��Ű�� �Լ�(Mathf.Lerp)
            textComponent.color = new Color(textColor.r, textColor.g, textColor.b, alpha); //�ؽ�Ʈ�� ����, ���� ����
            yield return null; //�� �������� ��ٸ� �� �ٽ� ����,�̰� �־�� �ε巴�� ������ ��ȭ��
        }

        textComponent.color = new Color(textColor.r, textColor.g, textColor.b, endAlpha);
    }
    void OnDisable()
    {
        isSceneActive = false; // ���� ����ǰų� ������Ʈ�� ��Ȱ��ȭ�Ǹ� ����
    }
}
