using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject enemyHPSliderPrefab;
    [SerializeField]
    private Transform canvasTransform;
    [SerializeField]
    private string defaultTargetTag = "Target"; // �⺻ Ÿ�� �±�
    [SerializeField]
    private GoodsBoxHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private float targetRefreshInterval = 5f; // Ÿ�� �ֱ��� ���� ����

    private Wave currentWave;
    private int currentEnemyCount;
    private List<Enemy> enemyList;
    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleSpawnPoints = new List<Vector3>();

    // �±׺� Ÿ�� ĳ�� (���� ����ȭ)
    private Dictionary<string, List<Transform>> taggedTargets = new Dictionary<string, List<Transform>>();

    // Ÿ�� �߰�/���� �̺�Ʈ
    public delegate void TargetEvent(string tag, Transform target);
    public event TargetEvent OnTargetAdded;
    public event TargetEvent OnTargetRemoved;

    // �� ����/���� �̺�Ʈ
    public delegate void EnemyEvent(Transform enemy);
    public event EnemyEvent OnEnemySpawned;
    public event EnemyEvent OnEnemyDestroyed;

    // ���� Ȱ��ȭ�� Ÿ�� �±� ����
    private HashSet<string> activeTargetTags = new HashSet<string>();

    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;

    private void Awake()
    {
        enemyList = new List<Enemy>();

        // Ÿ�ϸ��� �����Ǿ� ������ ������ ���� ��ġ ���
        if (tilemap != null)
        {
            CalculatePossibleSpawnPoints();
        }

        // ���� �� ��� �±� Ÿ���� ã�Ƽ� ĳ��
        CacheAllTaggedTargets();
    }

    private void Start()
    {
        // Ÿ�� ���� ������ ���� �ֱ������� Ȯ��
        StartCoroutine(PeriodicTargetRefresh());
    }

    private void OnEnable()
    {
        // �� �ε�/��ε� �� Ÿ�� ������ �����ϱ� ���� �̺�Ʈ ����
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �̺�Ʈ ���� ����
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // ���� �ε�� �� Ÿ�� ĳ�� ����
        CacheAllTaggedTargets();
    }

    // ���� ��� �±� Ÿ���� ã�Ƽ� ĳ���ϴ� �Լ�
    private void CacheAllTaggedTargets()
    {
        // ���� Ÿ�� ĳ�� �ʱ�ȭ
        taggedTargets.Clear();

        // �׻� �⺻ Ÿ�� �±״� ĳ��
        RefreshTaggedTargets(defaultTargetTag);

        // Ȱ��ȭ�� ��� �±׿� ���� ĳ��
        foreach (string tag in activeTargetTags)
        {
            RefreshTaggedTargets(tag);
        }
    }

    // Ư�� �±��� Ÿ���� �����ϴ� �Լ�
    private void RefreshTaggedTargets(string tag)
    {
        if (string.IsNullOrEmpty(tag)) return;

        // ���� ���� ����
        List<Transform> previousTargets = new List<Transform>();
        if (taggedTargets.ContainsKey(tag))
        {
            previousTargets.AddRange(taggedTargets[tag]);
        }

        // ���ο� Ÿ�� ��� ��������
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        List<Transform> newTargets = new List<Transform>();

        foreach (GameObject obj in taggedObjects)
        {
            if (obj != null)
            {
                newTargets.Add(obj.transform);
            }
        }

        // ��ųʸ� ������Ʈ
        taggedTargets[tag] = newTargets;

        // ������� Ȯ�� �� �̺�Ʈ �߻�
        // 1. �߰��� Ÿ�� Ȯ��
        foreach (Transform newTarget in newTargets)
        {
            if (!previousTargets.Contains(newTarget))
            {
                // �߰��� Ÿ�ٿ� ���� �̺�Ʈ �߻�
                OnTargetAdded?.Invoke(tag, newTarget);
                Debug.Log($"Target added: {newTarget.name} with tag {tag}");
            }
        }

        // 2. ���ŵ� Ÿ�� Ȯ��
        foreach (Transform oldTarget in previousTargets)
        {
            if (oldTarget != null && !newTargets.Contains(oldTarget))
            {
                // ���ŵ� Ÿ�ٿ� ���� �̺�Ʈ �߻�
                OnTargetRemoved?.Invoke(tag, oldTarget);
                Debug.Log($"Target removed: {oldTarget.name} with tag {tag}");
            }
        }
    }

    // �ֱ������� Ÿ�� ����� �����ϴ� �ڷ�ƾ
    private IEnumerator PeriodicTargetRefresh()
    {
        while (true)
        {
            // Ȱ��ȭ�� ��� �±׿� ���� Ÿ�� ����
            foreach (string tag in activeTargetTags)
            {
                RefreshTaggedTargets(tag);
            }

            // �⺻ �±׵� �׻� Ȯ��
            RefreshTaggedTargets(defaultTargetTag);

            yield return new WaitForSeconds(targetRefreshInterval);
        }
    }

    // Ư�� �±׸� ���� Ÿ���� ã�� �Լ�
    private Transform FindTargetByTag(string tag, Vector3 spawnPosition)
    {
        // �±װ� ����ְų� "Untagged"�� ��� �⺻ �±� ���
        if (string.IsNullOrEmpty(tag) || tag == "Untagged")
        {
            tag = defaultTargetTag;
        }

        // Ȱ�� �±� ��Ͽ� �߰�
        activeTargetTags.Add(tag);

        // �̹� ĳ�õ� �±� Ȯ��
        if (!taggedTargets.ContainsKey(tag))
        {
            // ĳ�ÿ� ������ ���� ã�Ƽ� �߰�
            RefreshTaggedTargets(tag);
        }

        // �ش� �±׸� ���� ������Ʈ ��� ��������
        List<Transform> targets = taggedTargets[tag];

        if (targets.Count == 0)
        {
            Debug.LogWarning($"�±� '{tag}'�� ���� ������Ʈ�� �����ϴ�. �⺻ �±׸� ����մϴ�.");

            // �⺻ �±׷� �ٽ� �õ�
            if (tag != defaultTargetTag)
            {
                return FindTargetByTag(defaultTargetTag, spawnPosition);
            }

            return null;
        }

        // ���� ����� Ÿ�� ����
        return FindNearestTarget(targets, spawnPosition);
    }

    // Ư�� ���� ���� Ÿ���� ���Ҵ��ϴ� �Լ�
    public Transform ReassignTargetForEnemy(Enemy enemy, string targetTag = null)
    {
        // ����� �±� ����
        string tagToUse = targetTag;
        if (string.IsNullOrEmpty(tagToUse))
        {
            // �±װ� �������� ���� ��� ���� ���̺��� �ش� �� �׷��� �±� ���
            if (currentWave.Equals(default(Wave)) == false)
            {
                foreach (var group in currentWave.enemyGroups)
                {
                    if (group.enemyPrefab.name == enemy.gameObject.name.Replace("(Clone)", ""))
                    {
                        tagToUse = string.IsNullOrEmpty(group.targetTag) ? defaultTargetTag : group.targetTag;
                        break;
                    }
                }
            }

            // ������ null�̸� �⺻ �±� ���
            if (string.IsNullOrEmpty(tagToUse))
            {
                tagToUse = defaultTargetTag;
            }
        }

        // ���� ���� ��ġ���� ���� ����� Ÿ�� ã��
        Transform newTarget = FindTargetByTag(tagToUse, enemy.transform.position);

        // �� Ÿ�� ����
        if (newTarget != null)
        {
            enemy.SetTarget(newTarget);
            Debug.Log($"Reassigned target for {enemy.name}: {newTarget.name}");
        }

        return newTarget;
    }

    // ��� ���� ���� Ÿ���� ���Ҵ��ϴ� �Լ�
    public void ReassignTargetsForAllEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                ReassignTargetForEnemy(enemy);
            }
        }
        Debug.Log($"Reassigned targets for all {enemyList.Count} enemies");
    }

    private void CalculatePossibleSpawnPoints()
    {
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // Check all tiles except the border tiles
        for (int y = 1; y < bounds.size.y - 1; ++y)
        {
            for (int x = 1; x < bounds.size.x - 1; ++x)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];
                if (tile != null)
                {
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    position.z = 0;
                    possibleSpawnPoints.Add(position);
                }
            }
        }
    }

    // Get spawn position based on provided spawn point or random position from tilemap
    public Vector3 GetSpawnPosition(Transform specificSpawnPoint = null)
    {
        // If a specific spawn point is provided, use it
        if (specificSpawnPoint != null)
        {
            return specificSpawnPoint.position;
        }

        // Otherwise use a random point from possible spawn points
        if (possibleSpawnPoints.Count > 0)
        {
            int index = Random.Range(0, possibleSpawnPoints.Count);
            return possibleSpawnPoints[index];
        }

        // Fallback to spawner position
        return transform.position;
    }

    // SpawnEnemyGroups �޼��� ����
    private IEnumerator SpawnEnemyGroups()
    {
        // ���̺��� �� �� �׷� ó��
        foreach (var enemyGroup in currentWave.enemyGroups)
        {
            // ���� ��ġ ���� (���� ����Ʈ�� �ִٸ� �ش� ��ġ, ������ �⺻ ������ ��ġ)
            Vector3 spawnPosition = enemyGroup.spawnPoint != null
                ? enemyGroup.spawnPoint.position
                : transform.position;

            // Ÿ�� �±׷� ���� Ÿ�� ã�� (���� ��ġ ����)
            string tagToUse = string.IsNullOrEmpty(enemyGroup.targetTag) ? defaultTargetTag : enemyGroup.targetTag;
            Transform targetToUse = FindTargetByTag(tagToUse, spawnPosition);

            // ��ȿ�� Ÿ���� ���� �׷��� �ǳʶٱ�
            if (targetToUse == null)
            {
                Debug.LogWarning($"������ {enemyGroup.enemyPrefab.name}�� ���� �� �׷��� �ǳʶݴϴ�. ��ȿ�� Ÿ���� �����ϴ�.");
                continue;
            }

            // �� �׷��� �� ���� ����
            yield return StartCoroutine(SpawnEnemyGroup(enemyGroup, targetToUse, tagToUse));
        }
    }

    private IEnumerator SpawnEnemyGroup(EnemyGroup enemyGroup, Transform target, string targetTag)
    {
        for (int i = 0; i < enemyGroup.count; i++)
        {
            // �⺻ ��ġ�� �� ���� (Setup���� ��ġ ������Ʈ��)
            GameObject clone = Instantiate(enemyGroup.enemyPrefab, transform.position, Quaternion.identity, transform);

            Enemy enemy = clone.GetComponent<Enemy>();

            // �پ缺�� ���� ���� ������ ����
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
            );
            enemy.SetSpawnOffset(randomOffset);

            // �� �׷��� ���� ����Ʈ�� ���� ���� ��ġ ������
            enemy.SetCustomSpawnPoint(enemyGroup.spawnPoint);

            // Ÿ�� �±� ���� (���� Ÿ�� ���Ҵ翡 ���)
            enemy.SetTargetTag(targetTag);

            // Ÿ�ٰ� �Բ� �� ����
            enemy.Setup(this, target);
            enemyList.Add(enemy);
            SpawnEnemyHPSlider(clone);

            // �̺�Ʈ �߻�
            OnEnemySpawned?.Invoke(enemy.transform);

            yield return new WaitForSeconds(enemyGroup.spawnTime);
        }
    }

    // ���̺��� Ÿ�� �±׸� �ٽ� �����ϴ� �Լ�
    private void RefreshTargetsForWave(Wave wave)
    {
        // ��� �±� ������Ʈ ĳ�� ����
        CacheAllTaggedTargets();

        // �� ���̺꿡�� ����ϴ� �±� ����
        foreach (var enemyGroup in wave.enemyGroups)
        {
            string tag = string.IsNullOrEmpty(enemyGroup.targetTag) ? defaultTargetTag : enemyGroup.targetTag;
            activeTargetTags.Add(tag);
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if (type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        enemyList.Remove(enemy);

        // �̺�Ʈ �߻�
        OnEnemyDestroyed?.Invoke(enemy.transform);

        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderclone = Instantiate(enemyHPSliderPrefab);
        sliderclone.transform.SetParent(canvasTransform, false);
        sliderclone.transform.localScale = Vector3.one;
        sliderclone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderclone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;

        // ���̺� ���� �� �±׵� Ÿ�� ����
        RefreshTargetsForWave(wave);

        // �� ���̺��� �� �� �� ���
        currentEnemyCount = 0;
        foreach (var enemyGroup in wave.enemyGroups)
        {
            // ���� ��ġ ����
            Vector3 spawnPosition = enemyGroup.spawnPoint != null
                ? enemyGroup.spawnPoint.position
                : transform.position;

            // ��ȿ�� �±װ� �ִ��� Ȯ��
            string tagToCheck = string.IsNullOrEmpty(enemyGroup.targetTag) ? defaultTargetTag : enemyGroup.targetTag;
            Transform target = FindTargetByTag(tagToCheck, spawnPosition);

            if (target != null)
            {
                currentEnemyCount += enemyGroup.count;
            }
        }

        StartCoroutine("SpawnEnemyGroups");
    }

    private Transform FindNearestTarget(List<Transform> targets, Vector3 spawnPosition)
    {
        if (targets == null || targets.Count == 0)
            return null;

        Transform nearestTarget = null;
        float minDistance = float.MaxValue;

        foreach (Transform target in targets)
        {
            // �̹� �ı��� ������Ʈ�� �ǳʶٱ�
            if (target == null)
                continue;

            float distance = Vector3.Distance(spawnPosition, target.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }

    // ��Ÿ�ӿ� �߰��� Ÿ���� ����ϴ� �Լ�
    public void RegisterTarget(string tag, Transform target)
    {
        if (string.IsNullOrEmpty(tag) || target == null) return;

        // Ȱ�� �±� ��Ͽ� �߰�
        activeTargetTags.Add(tag);

        // �ش� �±��� Ÿ�� ����� ������ ���� ����
        if (!taggedTargets.ContainsKey(tag))
        {
            taggedTargets[tag] = new List<Transform>();
        }

        // �̹� ��ϵ� Ÿ���� �ƴ� ��쿡�� �߰�
        if (!taggedTargets[tag].Contains(target))
        {
            taggedTargets[tag].Add(target);

            // �̺�Ʈ �߻�
            OnTargetAdded?.Invoke(tag, target);

            Debug.Log($"Target manually registered: {target.name} with tag {tag}");

            // �ش� �±׸� ����ϴ� ������ Ÿ���� ���Ҵ��� �ʿ䰡 �ִ��� Ȯ��
            CheckEnemiesForTargetReassignment(tag);
        }
    }

    // ��Ÿ�ӿ� ���ŵ� Ÿ���� ��� �����ϴ� �Լ�
    public void UnregisterTarget(string tag, Transform target)
    {
        if (string.IsNullOrEmpty(tag) || target == null) return;

        if (taggedTargets.ContainsKey(tag) && taggedTargets[tag].Contains(target))
        {
            taggedTargets[tag].Remove(target);

            // �̺�Ʈ �߻�
            OnTargetRemoved?.Invoke(tag, target);

            Debug.Log($"Target manually unregistered: {target.name} with tag {tag}");

            // �ش� �±׸� ����ϴ� ������ Ÿ���� ���Ҵ��� �ʿ䰡 �ִ��� Ȯ��
            CheckEnemiesForTargetReassignment(tag);
        }
    }

    // Ư�� �±׸� ����ϴ� ������ Ÿ�� ���Ҵ� �ʿ伺 Ȯ��
    private void CheckEnemiesForTargetReassignment(string tag)
    {
        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null && enemy.TargetTag == tag)
            {
                // ���� Ÿ���� ���ų� �ı��� ��쿡�� ���Ҵ�
                if (enemy.CurrentTarget == null || taggedTargets[tag].Count == 0)
                {
                    ReassignTargetForEnemy(enemy, tag);
                }
            }
        }
    }
}