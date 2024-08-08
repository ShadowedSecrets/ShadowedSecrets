using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthUI2 : MonoBehaviour
{
    public GameObject healthBarObject; 
    public GameObject bossNameTextObject;

    private Slider healthBar; 
    private TextMeshProUGUI bossNameText;
    private BossLevel2 boss;
    

    void Start()
    {
        healthBar = healthBarObject.GetComponent<Slider>();
        bossNameText = bossNameTextObject.GetComponent<TextMeshProUGUI>();

        healthBar.gameObject.SetActive(false);
        ConfigureTextComponent();
    }

    public void Initialize(BossLevel2 boss)
    {
        this.boss = boss;
        
        healthBar.maxValue = boss.maxHealth;
        healthBar.value = boss.maxHealth;
        healthBar.gameObject.SetActive(true);
    }

    void Update()
    {
        if (boss != null)
        {
            healthBar.value = boss.GetCurrentHealth();
        }
    }

    public void Hide()
    {
        healthBar.gameObject.SetActive(false);
    }

    private void ConfigureTextComponent()
    {
        RectTransform rt = bossNameText.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 50); 

        bossNameText.alignment = TextAlignmentOptions.Center;
        bossNameText.enableWordWrapping = false;
        bossNameText.overflowMode = TextOverflowModes.Overflow;
    }
}







