using UnityEngine;

public class ProjectileQuadraticHoming : ProjectileBase
{
	private	Vector2		start, end, point;
	private	float		duration, t = 0f;
	private	Transform	target;

	public override void Setup(Transform target, float damage, int maxCount = 1, int index = 0)
	{
		base.Setup(target, damage);

		this.target	= target;
		start		= transform.position;
		end			= this.target.position;

		// ���� �������� ��ǥ������ �Ÿ� ���
		float distance = Vector3.Distance(start, end);
		// ��� �ð� ���� (�Ÿ� / �̵��ӵ�)
		duration = distance / movementRigidbody2D.MoveSpeed;

		// ��� �߻�ü�� point�� �����ϰ� 45�� ���� ��ġ�� ����
		//float angle = 45;

		// ������ ���� ������ ������ �������� ��ġ ����
		//float angle = 360 / maxCount * index;

		// ������ ���� �� or �Ʒ� �밢��(45 or 315��) ��ġ�� ����
		float angle = 45 + 270 * (index % 2);

		// ���� �÷��̾��� ȸ�� �� ������ ���� angle ���� �����ش�
		angle += Utils.GetAngleFromPosition(start, end);

		// ������������ ��ǥ���� ������ angle ������ 30% ������ ��ġ
		point = Utils.GetNewPoint(start, angle, distance * 0.3f);

		// point ��ġ Ȯ���� ���� ����� �ڵ� [��� Ȯ�� �� ����]
		GameObject clone = Instantiate(gameObject, point, Quaternion.identity);
		clone.GetComponent<ProjectileQuadraticHoming>().enabled = false;
		clone.GetComponentInChildren<SpriteRenderer>().color = Color.black;
	}

public override void Process()
{
    if (target == null)
    {
        Debug.LogError("Target is null!");
        Destroy(gameObject);
        return;
    }

    end = target.position;
    t += Time.deltaTime / duration;
    t = Mathf.Clamp01(t); // t ���� ����

    if (float.IsNaN(t) || float.IsInfinity(t))
    {
        Debug.LogError($"Invalid t value: {t}");
        return;
    }

    transform.position = Utils.QuadraticCurve(start, point, end, t);
}

}

