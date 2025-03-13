using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
        List<Vector3> path = new List<Vector3>();

        // ������ A* �˰���
        Vector3[] directions = { Vector3.right, Vector3.left, Vector3.up, Vector3.down }; // 2D ��� �̵�

        // ť�� �湮 ���
        Queue<Vector3> frontier = new Queue<Vector3>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
        frontier.Enqueue(start);
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            Vector3 current = frontier.Dequeue();

            if (current == target)
            {
                // ��θ� ã���� �����Ͽ� ��ȯ
                Vector3 temp = target;
                while (!temp.Equals(start))
                {
                    path.Add(temp);
                    temp = cameFrom[temp];
                }
                path.Add(start);
                path.Reverse(); // ��� ������ �ݴ��
                return path;
            }

            foreach (Vector3 direction in directions)
            {
                Vector3 next = current + direction;

                // Ÿ���� ���̳� ��ȿ���� ������ �Ѿ��
                if (IsValid(next) && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        return path; // ��θ� ã�� ������ ��� �� ����Ʈ ��ȯ
    }

    // �� üũ ���� (������, ���ӿ� ���缭 ���� �ʿ�)
    private static bool IsValid(Vector3 position)
    {
        // ���� ���ϰų�, ���� ������ ������ �ʵ��� ó��
        return position.x >= 0 && position.y >= 0; // ���� ����
    }
}
