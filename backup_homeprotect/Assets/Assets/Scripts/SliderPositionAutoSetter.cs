using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
   
    [SerializeField]
    private Vector3 distance = Vector3.up * 20.0f;
    private Transform targetTransform;
    private RectTransform rectTransform; // Slider�� RectTransform
    public void Setup(Transform target)
    {
        targetTransform = target; // ��� ����
        rectTransform = GetComponent<RectTransform>(); // RectTransform ��������

    }

    private void LateUpdate()
    {
        if (targetTransform == null )
        {
            Destroy(gameObject);
            return;
        }

        // ���� World Position�� Canvas�� UI Position���� ��ȯ
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        // UI Canvas �������� ��ȯ�� ��ǥ�� RectTransform�� �ݿ�
        rectTransform.position = screenPosition + distance;
    }
}
