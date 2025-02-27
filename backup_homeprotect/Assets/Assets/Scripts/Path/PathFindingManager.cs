using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
        // ������ A* �˰����� ���� (�⺻���� ���)
        List<Vector3> path = new List<Vector3>();

        // ���⿡ A* �˰��� ������ �ۼ��ϼ���.
        // �� ���������� �ܼ��� �������� ��ǥ���� �߰��մϴ�.
        path.Add(start);
        path.Add(target);

        return path;
    }
}
