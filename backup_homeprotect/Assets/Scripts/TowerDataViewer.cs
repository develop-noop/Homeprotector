using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamge;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
       

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        if (towerWeapon == null)
        {
            Debug.LogWarning("������ Ÿ���� �����ϴ�.");
            return;
        }
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();
    }
    public void OffPanel()
    {
        gameObject.SetActive(false);
    }
    private void UpdateTowerData()
    {
        if (currentTower == null)
        {
            Debug.LogWarning("������Ʈ�� Ÿ�� ������ �����ϴ�.");
            OffPanel();
            return;
        }

        imageTower.sprite = currentTower.TowerSprite;
        textDamge.text = "Damage" + currentTower.Damage;
        textRate.text = "Rate" + currentTower.Rate;
        textRange.text = "Range" + currentTower.Range;
        textLevel.text = "Level" + currentTower.Level;

        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel? true:false;

    }

    public void OnClickEventTowerUpgrade()
    {
        if (currentTower == null)
        {
            Debug.LogWarning("���׷��̵��� Ÿ���� �����ϴ�.");
            return;
        }

        bool isSuccess = currentTower.Upgrade();
        if (isSuccess)
        {
            UpdateTowerData();

        }
        else 
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        if (currentTower == null)
        {
            Debug.LogWarning("current Tower is null");
            return;
        }

        currentTower.Sell();
        OffPanel();
    }
 }
