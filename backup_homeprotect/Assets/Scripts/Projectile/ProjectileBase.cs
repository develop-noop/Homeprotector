using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
	[SerializeField]
	private		GameObject			hitEffect;
	protected	MovementRigidbody2D	movementRigidbody2D;

	public virtual void Setup(Transform target, float damage, int maxCount=1, int index=0)
	{
		movementRigidbody2D = GetComponent<MovementRigidbody2D>();
	}

	private void Update()
	{
		Process();
	}

	public abstract void Process();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ( collision.CompareTag("Enemy") )
		{
			Instantiate(hitEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);

			// �� ĳ���� �ǰ� ó��
		}
	}
}

