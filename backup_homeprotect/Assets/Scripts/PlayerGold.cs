
using UnityEngine;

public class PlayerGold : MonoBehaviour//�÷��̾� ���� ��ǥ ����
{
    [SerializeField]
    private int currentGold = 300;
    public int playerLevel = 1;// ������ ���� ������
    public float playerFatigue = 0; //�Ƿε�

    public int CurrentGold
    { 
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }
}
