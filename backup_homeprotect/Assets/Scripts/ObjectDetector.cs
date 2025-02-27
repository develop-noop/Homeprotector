using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
   // private Transform hitTransform = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f; // 2D ȯ�濡�� Z ���� ����
            // Raycast �߻�
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);


            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Tower")) // Ÿ���� Ŭ���� ���
                {
                    Debug.Log("Tower clicked! Opening panel...");
                    towerDataViewer.OnPanel(hit.transform);
                    return; // Ÿ�� Ŭ�� �� Ÿ�ϸ� ���� ���� ���� X
                }
            }

            // Raycast ��� ó��
            Vector3Int cellPosition = towerSpawner.GetTilemap().WorldToCell(worldPosition);

            if (towerSpawner.IsTileOccupied(cellPosition))
            {
                // Ÿ���� �̹� �����ϴ� ��� -> ����
                Debug.Log($"Removing tower at {cellPosition}");

            }
            else
            {
                // Ÿ���� ���� ��� -> ����
                Debug.Log($"Spawning tower at {cellPosition}");
                towerSpawner.SpawnTower(cellPosition);
            }
        }
       
    }

}
