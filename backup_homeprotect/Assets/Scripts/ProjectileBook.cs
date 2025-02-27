using UnityEngine;

public class ProjectileBook : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    private bool isInitialized = false; // �ʱ�ȭ ���� Ȯ�� �÷���
    private SpriteRenderer spriteRenderer;

    public Sprite[] projectileSprites; // 3���� �߻�ü �̹���
    private static int spriteIndex = 0; // ���������� �����Ǵ� �ε���

    public void Setup(Transform target, float damage)
    {
        movement2D = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.target = target;
        this.damage = damage;
        isInitialized = true; // �ʱ�ȭ �Ϸ�

        // ��������Ʈ ���� (����������)
        if (projectileSprites.Length > 0)
        {
            spriteRenderer.sprite = projectileSprites[spriteIndex];
            spriteIndex = (spriteIndex + 1) % projectileSprites.Length; // ���� �̹����� ��ȯ
        }
    }

    private void Update()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Projectile not initialized. Waiting for setup...");
            return; // �ʱ�ȭ ������ �������� ����
        }

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Debug.Log(target.position);
            movement2D.MoveTo(direction);

            if (direction == Vector3.zero)
            {
                Debug.LogWarning("Direction is zero. Not moving.");
                return;
            }
            else
            {
                Debug.Log("moveDirection set");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != target) return;

        collision.GetComponent<EnemyHP>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
