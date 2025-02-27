using UnityEngine;

public class ProjectileSpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite[] projectileSprites; // 3���� �߻�ü �̹���
    private static int spriteIndex = 0; // ���������� �����Ǵ� �ε���

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ��������Ʈ ���� (����������)
        if (projectileSprites.Length > 0)
        {
            spriteRenderer.sprite = projectileSprites[spriteIndex];
            spriteIndex = (spriteIndex + 1) % projectileSprites.Length; // ���� �̹����� ��ȯ
        }
        else
        {
            Debug.LogWarning("No projectile sprites assigned.");
        }
    }
}
