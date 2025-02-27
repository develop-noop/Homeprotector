using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyHP enemyHP; // �� HP ������ �����ϴ� ����
    private Slider hpSlider; // UI�� Slider ������Ʈ

    /// <summary>
    /// EnemyHPViewer ���� �޼���. EnemyHP�� Slider�� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="enemyHP">�� HP�� �����ϴ� EnemyHP ��ü</param>
    public void Setup(EnemyHP enemyHP)
    {
        this.enemyHP = enemyHP;
        hpSlider = GetComponent<Slider>();
        hpSlider.gameObject.SetActive(false);
        enemyHP.onTakeDamage += ShowHPBar;
        if (hpSlider == null)
        {
            Debug.LogError("Slider ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�. EnemyHPViewer�� Slider�� �ִ� ������Ʈ�� �پ� �־�� �մϴ�.");
        }
    }

    // Update �޼��忡�� Slider ���� ����
    public void Update()
    {
        if (enemyHP == null || hpSlider == null)
        {
            Debug.LogWarning("enemyHP �Ǵ� hpSlider�� null �����Դϴ�. Setup�� ȣ��Ǿ����� Ȯ���ϼ���.");
            return;
        }

        // Slider �� ������Ʈ
        hpSlider.value = enemyHP.CurrentHP / enemyHP.MaxHp;
    }
    private void ShowHPBar()
    {
        hpSlider.gameObject.SetActive(true);

        // �� �� �� ��Ȱ��ȭ
        StartCoroutine(HideHPBar());
    }

         private IEnumerator HideHPBar()
    {
        yield return new WaitForSeconds(3.0f); // 3�� ��
        hpSlider.gameObject.SetActive(false);
    }
}

