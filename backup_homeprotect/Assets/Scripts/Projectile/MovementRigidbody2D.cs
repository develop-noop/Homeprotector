using UnityEngine;

public class MovementRigidbody2D : MonoBehaviour
{
	[SerializeField]
	private	float		moveSpeed = 10f;
	private	Rigidbody2D	rigid2D;

	public	float		MoveSpeed => moveSpeed;

	private void Awake()
	{
		rigid2D = GetComponent<Rigidbody2D>();
	}

	public void MoveTo(Vector3 direction)
	{
		rigid2D.velocity = direction * moveSpeed;
	}
}

