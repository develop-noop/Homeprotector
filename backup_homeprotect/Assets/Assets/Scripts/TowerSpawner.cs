using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;
public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private List<TowerTemplate> towerTemplates;// ���� ������ Ÿ�� ���ø�

    [SerializeField]
    private EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ����

    [SerializeField]
    private Grid grid; // Ÿ�ϸ��� ���� Grid ������Ʈ

    [SerializeField]
    private PlayerGold playerGold;

    [SerializeField]
    private SystemTextViewer systemTextViewer;


    [SerializeField]
    private Tilemap tilemap;


    // ��ġ�� Ÿ������ ���� ��ǥ(�� ��ǥ) �������� ����
    private Dictionary<Vector3Int, GameObject> placedTowers = new Dictionary<Vector3Int, GameObject>();

    // ���� ���õ� Ÿ�� ���� �ε��� (Ÿ�� ���ø� ����Ʈ ���� �ε���)
    private int selectedTowerIndex = 0;

    // Ÿ�� ��ġ ��� Ȱ��ȭ ����
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;

    public Tilemap GetTilemap() => tilemap;


    // ������ Ÿ�� ������ �����ϰ� ��ġ ��忡 ��
    public void SelectAndReadyTower(int index)
    {
        if (index < 0 || index >= towerTemplates.Count)
        {
            Debug.LogError("�߸��� Ÿ�� �ε���");
            return;
        }

        selectedTowerIndex = index;

        // ��ġ ��� ���� ���� ��� üũ
        if (towerTemplates[selectedTowerIndex].weapons[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        isOnTowerButton = true;
        // ������ Ÿ���� followTowerPrefab�� �����Ͽ� �̸� ��ġ �̸����� ����
        followTowerClone = Instantiate(towerTemplates[selectedTowerIndex].followTowerPrefab);
        // �ʿ��ϴٸ� followTowerClone�� ��ġ �� ��Ÿ ������ ���⼭ ����
        StartCoroutine(OnTowerCancelSystem());
    }

    public void ReadyToSpawnTower()
    {
        if (isOnTowerButton)
        {
            return;
        }
        TowerTemplate selectedTower = towerTemplates[selectedTowerIndex];

        if (selectedTower.weapons[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        isOnTowerButton = true;
        followTowerClone = Instantiate(selectedTower.followTowerPrefab);// �ӽ� Ÿ�� ����
        StartCoroutine("OnTowerCancelSystem");
    }

    void Update()
    {
        if (isOnTowerButton && followTowerClone != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            followTowerClone.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }

    public void SpawnTower(Vector3Int cellPosition)
    {
        if (isOnTowerButton == false)
        {
            return;
        }
        if (IsTileOccupied(cellPosition)) return; // �̹� Ÿ���� �����ϸ� ���� X

        TowerTemplate selectedTower = towerTemplates[selectedTowerIndex];

        if (selectedTower.weapons[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        
        Vector3 towerPosition = tilemap.GetCellCenterWorld(cellPosition); // Ÿ�� �߽��� ���� ��ǥ
        // Isometric z as y
       // towerPosition.z = towerPosition.y;

        GameObject newTower = Instantiate(selectedTower.towerPrefab, towerPosition, Quaternion.identity); // Ÿ�� ����
        TowerWeapon towerWeapon = newTower.GetComponent<TowerWeapon>();
        
        if (towerWeapon != null)
        {
            towerWeapon.Setup(selectedTower, enemySpawner, playerGold, towerPosition); // Setup ȣ��
        }
        isOnTowerButton = false;
        placedTowers[cellPosition] = newTower; // �� ��ǥ�� Ÿ�� ����
        playerGold.CurrentGold -= selectedTower.weapons[0].cost; // ��� ����
        
        StopCoroutine("OnTowerCancelSystem");
        Destroy(followTowerClone);
       

        Debug.Log($"Tower placed at {cellPosition}");
    }

    public void RemoveTower(Vector3Int cellPosition)
    {
        if (placedTowers.TryGetValue(cellPosition, out GameObject tower)) // Ÿ�� ã��
        {
            Destroy(tower); // Ÿ�� ������Ʈ ����
            placedTowers.Remove(cellPosition); // ���� ��Ͽ��� ����

            Debug.Log($"Tower removed from {cellPosition}");
        }
        else
        {
            Debug.Log("No tower found at the specified position.");
        }
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }
            yield return null; 
        }
    }

    public bool IsTileOccupied(Vector3Int cellPosition)
    {
        return placedTowers.ContainsKey(cellPosition);
    }



}

