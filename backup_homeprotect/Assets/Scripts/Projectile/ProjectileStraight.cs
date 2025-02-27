using UnityEngine;

public class ProjectileStraight : ProjectileBase
{
	public override void Setup(Transform target, float damage, int maxCount = 1, int index = 0)
	{
		base.Setup(target, damage);

		// �߻�ü �̵� ���� ����
		movementRigidbody2D.MoveTo((target.position - transform.position).normalized);
	}

	public override void Process() { }
}

