using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int gold = 10; // �� ����� ȹ�� ���

    private Transform target;
    private NavMeshAgent navMeshAgent;
    private EnemySpawner enemySpawner;
    private Vector3 spawnOffset = Vector3.zero;
    private Transform customSpawnPoint = null;
    private string targetTag = "Target"; // �⺻ Ÿ�� �±� ����

    // ���� Ÿ�ٿ� ���� ������ �߰�
    public Transform CurrentTarget => target;
    public string TargetTag => targetTag;

    public void SetSpawnOffset(Vector3 offset)
    {
        spawnOffset = offset;
    }

    public void SetCustomSpawnPoint(Transform spawnPoint)
    {
        customSpawnPoint = spawnPoint;
    }

    public void SetTargetTag(string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            targetTag = tag;
        }
    }

    // Ÿ���� �����ϴ� �޼��� �߰�
    public void SetTarget(Transform newTarget)
    {
        if (newTarget != null && newTarget != target)
        {
            target = newTarget;

            // NavMeshAgent ��� �缳��
            if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.SetDestination(target.position);
            }

            Debug.Log($"Enemy {gameObject.name} target changed to {target.name}");
        }
    }

    public void Setup(EnemySpawner spawner, Transform target)
    {
        enemySpawner = spawner;
        this.target = target;

        // NavMeshAgent ����
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }
        else
        {
            Debug.LogError("NavMeshAgent component not found on enemy!");
        }

        // ���� ��ġ ����
        SetSpawnPosition();

        // Ÿ�� �߰�/���� �̺�Ʈ ����
        if (enemySpawner != null)
        {
            enemySpawner.OnTargetAdded += HandleTargetAdded;
            enemySpawner.OnTargetRemoved += HandleTargetRemoved;
        }

        StartCoroutine("OnMove");
    }

    // ���� ��ġ�� ����
    private void SetSpawnPosition()
    {
        // �⺻ ���� ��ġ ��������
        Vector3 spawnPosition;
        if (customSpawnPoint != null)
        {
            spawnPosition = customSpawnPoint.position;
        }
        else
        {
            spawnPosition = enemySpawner.GetSpawnPosition();
        }

        // ���� ������ ����
        spawnPosition += spawnOffset;

        // NavMesh ��ġ�� ���� (���� ����� NavMesh ���� ã��)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("NavMesh ������ ã�� �� �����ϴ�. ���� ��ġ�� �����˴ϴ�.");
            transform.position = spawnPosition;
        }
    }

    // Ÿ���� �߰��Ǿ��� �� ȣ��Ǵ� �ڵ鷯
    private void HandleTargetAdded(string tag, Transform newTarget)
    {
        // ���� ��� ���� �±׿� ��ġ�ϰ�, ���� Ÿ���� ���� ��쿡�� ó��
        if (tag == targetTag && (target == null || !target.gameObject.activeInHierarchy))
        {
            // ���� ��ġ���� ���� ����� �� Ÿ������ ����
            enemySpawner.ReassignTargetForEnemy(this, tag);
        }
    }

    // Ÿ���� ���ŵǾ��� �� ȣ��Ǵ� �ڵ鷯
    private void HandleTargetRemoved(string tag, Transform removedTarget)
    {
        // ���� ��� ���� �±װ� ��ġ�ϰ�, ���� Ÿ���� ���ŵ� Ÿ���� ���
        if (tag == targetTag && target == removedTarget)
        {
            // ���ο� Ÿ�� ã��
            Transform newTarget = enemySpawner.ReassignTargetForEnemy(this, tag);

            // ��ȿ�� Ÿ���� �� �̻� ���� ��� ó��
            if (newTarget == null)
            {
                Debug.LogWarning($"Enemy {gameObject.name} has no valid target after target removal");
                // ������: ���� �����ϰų� ��� ���·� ����
            }
        }
    }

    private void OnDisable()
    {
        // �̺�Ʈ ���� ����
        if (enemySpawner != null)
        {
            enemySpawner.OnTargetAdded -= HandleTargetAdded;
            enemySpawner.OnTargetRemoved -= HandleTargetRemoved;
        }
    }

    private IEnumerator OnMove()
    {
        if (target == null)
        {
            // Ÿ���� ���� ��� �� Ÿ�� ã�� �õ�
            if (enemySpawner != null)
            {
                Transform newTarget = enemySpawner.ReassignTargetForEnemy(this, targetTag);
                if (newTarget == null)
                {
                    Debug.LogError("Target is not assigned for enemy: " + gameObject.name);
                    yield break;
                }
            }
            else
            {
                Debug.LogError("EnemySpawner is not assigned for enemy: " + gameObject.name);
                yield break;
            }
        }

        // ���� ��ǥ������ ���� �̵�
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(target.position);
        }

        while (true)
        {
            // Ÿ���� �ı��� ��� Ȯ��
            if (target == null)
            {
                // �� Ÿ�� �Ҵ� �õ�
                Transform newTarget = enemySpawner.ReassignTargetForEnemy(this, targetTag);
                if (newTarget == null)
                {
                    // ��ȿ�� Ÿ���� ������ �� ����
                    OnDie(EnemyDestroyType.Kill);
                    yield break;
                }
            }

            // ��ǥ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                gold = 0; // ��ǥ ���� �� ���� 0
                OnDie(EnemyDestroyType.Arrive);
                yield break;
            }

            // ��ǥ�� ������ �� �����Ƿ� ���������� ��ǥ ��ġ ������Ʈ
            if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled && target != null)
            {
                navMeshAgent.SetDestination(target.position);
            }

            yield return null;
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner is not assigned! Check the Setup method.");
            return; // NullReferenceException ����
        }

        enemySpawner.DestroyEnemy(type, this, gold);
    }
}