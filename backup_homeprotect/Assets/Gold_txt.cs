using UnityEngine;
using UnityEngine.UI;

public class Gold_txt : MonoBehaviour
{
    [SerializeField]
    private int currentGold = 300;

    public Text goldText;

    public int CurrentGold
    {
        get => currentGold;
        set
        {
            currentGold = Mathf.Max(0, value);
            UpdateGoldText();
        }
    }

    void Start()
    {
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        goldText.text = "Gold: " + CurrentGold.ToString();
    }
}
