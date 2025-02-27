using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private Button[] towerButtons; // �� ��ư�� Ÿ�� ������ ��ǥ

    void Start()
    {
        // ��ư�� ������ towerTemplates�� ������ ��ġ�Ѵٰ� ����
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; // �������� ĸó
            towerButtons[i].onClick.AddListener(() => OnTowerButtonClicked(index));
        }
    }

    private void OnTowerButtonClicked(int index)
    {
        // ������ Ÿ�� ������ �����ϰ� ��ġ ��忡 ����
        towerSpawner.SelectAndReadyTower(index);
    }
}
