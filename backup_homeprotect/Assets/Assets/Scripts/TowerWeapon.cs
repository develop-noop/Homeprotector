using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget } //���� ��� Ž�� ����

public class TowerWeapon : MonoBehaviour
{
  
    [SerializeField]
    private GameObject projectilePrefab; // �߻�ü ������
    [SerializeField]
    private Transform spawnPoint;

    
    private TowerTemplate towerTemplate;
    private int level = 0;
    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpriteRenderer spriteRenderer;
    private PlayerGold playerGold;
    private EnemySpawner enemySpawner;
    private Tile ownerTile;

    public Sprite TowerSprite => towerTemplate.weapons[level].sprite;
    public float Damage => towerTemplate.weapons[level].damage;
    public float Rate => towerTemplate.weapons[level].rate;
    public float Range => towerTemplate.weapons[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapons.Count;

    public void Setup(TowerTemplate template, EnemySpawner enemySpawner, PlayerGold playerGold, Vector3 worldPosition)
    {
        towerTemplate = template;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("TowerWeapon Setup called!");
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.transform.position = worldPosition;
        spriteRenderer.sprite = towerTemplate.weapons[level].sprite;
        ChangeState(WeaponState.SearchTarget);
    }


    public void ChangeState(WeaponState newstate)
    {
        Debug.Log($"Changing state to {newstate}");
        StopCoroutine(weaponState.ToString());
        weaponState = newstate;
        StartCoroutine(weaponState.ToString());
    }
    // Update is called once per frame
    private void Update()
    {
        if (attackTarget != null)
        {
            RotateToTarget();
        }

    }

    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float closetDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++) //��� �� �˻�
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                if (distance <= towerTemplate.weapons[level].range && distance <= closetDistSqr)
                {
                    closetDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }
            if (attackTarget != null)
            {
                Debug.Log($"Target found: {attackTarget.name}");
                ChangeState(WeaponState.AttackToTarget); // �ش� Ÿ�� ����
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            if (attackTarget == null) // target �ִ��� Ȯ��
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > towerTemplate.weapons[level].range) //target�� ���� �������� �� ��� ���ο� �� Ž��
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(towerTemplate.weapons[level].rate);

            SpawnProjectile(); // �߻�ü ����
        }
    }

    private void SpawnProjectile()
    {
        Debug.Log($"Spawning projectile at {spawnPoint.position}");
        GameObject projectileObj = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        // Get the Projectile component from the spawned object
        Projectile projectileScript = projectileObj.GetComponent<Projectile>();

        // Make sure the projectileScript is not null
        if (projectileScript != null)
        {
            // Call Setup and pass the target and attack damage to the projectile
            projectileScript.Setup(attackTarget, towerTemplate.weapons[level].damage);
            Debug.Log("Projectile setup complete.");
        }
        else
        {
            Debug.LogError("Projectile component not found on the spawned object!");
        }


    }

    public bool Upgrade()
    {
        if (level + 1 >= towerTemplate.weapons.Count || playerGold.CurrentGold < towerTemplate.weapons[level + 1].cost)
        { 
            return false; 

        }
        level++;
        spriteRenderer.sprite = towerTemplate.weapons[level].sprite;
        playerGold.CurrentGold -= towerTemplate.weapons[level].cost;

        return true;
    }

    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapons[level].sell;
        
        Vector3Int cellposition = FindObjectOfType<Grid>().WorldToCell(transform.position);
        FindObjectOfType<TowerSpawner>().RemoveTower(cellposition);

        Destroy(gameObject);
    }
}

