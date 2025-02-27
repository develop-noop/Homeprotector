using UnityEngine;

public enum ProjectileType { Straight, Homing, QuadraticHoming, CubicHoming }

public class SkillEmission : MonoBehaviour
{
	[SerializeField]
	private	ProjectileType	projectileType = ProjectileType.Straight;
	[SerializeField]
	private	int				projectileCount = 10;
	[SerializeField]
	private	float			cooldownTime = 2f;
	[SerializeField]
	private	GameObject[]	projectiles;
	[SerializeField]
	private	Transform		skillSpawnPoint;
	[SerializeField]
	private	Transform		target;

	private	int				currentProjectileIndex = 0;
	private	float			currentAttackRate = 0;
	private	float			currentCooldownTime = 0;
	private	float			attackRate = 0.05f;		// �߻�ü ������ ����

	public	bool			IsSkillAvailable => (Time.time - currentCooldownTime > cooldownTime);

	private void Update()
	{
		OnSkill();
	}

	public void OnSkill()
	{
		// ��ų�� ��� ������ �������� �˻� (��Ÿ��)
		if ( IsSkillAvailable == false ) return;

		// attackRate �ֱ�� �߻�ü ����
		if ( Time.time - currentAttackRate > attackRate )
		{
			GameObject clone = GameObject.Instantiate(projectiles[(int)projectileType], skillSpawnPoint.position, Quaternion.identity);
			clone.GetComponent<ProjectileBase>().Setup(target, 1, projectileCount, currentProjectileIndex);

			currentProjectileIndex ++;
			currentAttackRate = Time.time;
		}

		// ProjectileCount ������ŭ �߻�ü�� ������ �� ��Ÿ�� �ʱ�ȭ
		if ( currentProjectileIndex >= projectileCount )
		{
			currentProjectileIndex	= 0;
			currentCooldownTime		= Time.time;
		}
	}
}

