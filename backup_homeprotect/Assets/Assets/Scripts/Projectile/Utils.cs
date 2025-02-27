using UnityEngine;

public static class Utils
{
	public static float GetAngleFromPosition(Vector2 owner, Vector2 target)
	{
		// �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
		// ���� = arctan(y/x)
		// x, y ������ ���ϱ�
		float dx = target.x - owner.x;
		float dy = target.y - owner.y;

		// x, y �������� �������� ���� ���ϱ�
		// ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
		float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

		return degree;
	}

	/// <summary>
	/// Degree ���� Radian ������ ��ȯ
	/// 1���� "PI/180" radian
	/// angle���� "PI/180 * angle" radian
	/// </summary>
	public static float DegreeToRadian(float angle)
	{
		return Mathf.PI * angle / 180;
	}

	public static Vector2 GetNewPoint(Vector3 start, float angle, float r)
	{
		// Degree ���� ���� Radian���� ����
		angle = DegreeToRadian(angle);

		// ������ �������� x, y ��ǥ�� ���ϱ� ������ �������� ��ǥ(start)�� �����ش�
		Vector2 position = Vector2.zero;
		position.x = Mathf.Cos(angle) * r + start.x;
		position.y = Mathf.Sin(angle) * r + start.y;

		return position;
	}

	public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
	{
		return a + (b - a) * t;
	}

	public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
	{
		Vector2 p1 = Lerp(a, b, t);
		Vector2 p2 = Lerp(b, c, t);

		return Lerp(p1, p2, t);
	}

	public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
	{
		Vector2 p1 = QuadraticCurve(a, b, c, t);
		Vector2 p2 = QuadraticCurve(b, c, d, t);

		return Lerp(p1, p2, t);
	}
}

