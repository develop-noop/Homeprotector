using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;
    [SerializeField]
    private int gold = 10;// �� ����� ȹ�� ���

    public void Setup(EnemySpawner spawner, Transform[] wayPoints)
    {
        enemySpawner = spawner;
        movement2D = GetComponent<Movement2D>();
        movement2D = GetComponent<Movement2D>();

        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;


        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo(); // ���� ��ǥ��ġ

        while (true)
        {
            //transform.Rotate(Vector3.forward * 10);

            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        if (currentIndex < wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            gold = 0;
            OnDie(EnemyDestroyType.Arrive);
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
        Debug.Log($"{gameObject.name} died!");
    }
}