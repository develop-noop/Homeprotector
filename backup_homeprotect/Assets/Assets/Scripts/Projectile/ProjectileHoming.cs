using UnityEngine;

public class ProjectileHoming : ProjectileBase
{
	private Transform target;

	public override void Setup(Transform target, float damage, int maxCount = 1, int index = 0)
	{
		base.Setup(target, damage);

		this.target = target;
	}

	public override void Process()
	{
		// �߻�ü �̵� ���� ����
		movementRigidbody2D.MoveTo((target.position - transform.position).normalized);
	}
}

