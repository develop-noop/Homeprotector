using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField]
    private string targetTag; // �±� ����

    public string TargetTag => targetTag;

    private void Awake()
    {
        gameObject.tag = targetTag; // �±� �ڵ� ����
    }
}
