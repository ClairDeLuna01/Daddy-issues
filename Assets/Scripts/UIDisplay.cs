using UnityEngine;
using UnityEngine.UIElements;

public class UIDisplay : MonoBehaviour
{
    private Player player;
    private VisualElement root;

    public GameManager gameManager;

    ProgressBar bossHealth;
    VisualElement bossHealthElement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        root = GetComponent<UIDocument>().rootVisualElement;
        string[] classNames = { "unity-progress-bar__progress", "energy" };
        var actualBar = root.Q(classes: classNames);
        actualBar.style.backgroundColor = Color.cyan;
        classNames = new string[] { "unity-progress-bar__background", "energy" };
        var barBackground = root.Q(classes: classNames);
        barBackground.style.backgroundColor = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        Label armorIcon = root.Q<Label>("armor-icon");
        ProgressBar energyBar = root.Q<ProgressBar>("energy-bar");

        energyBar.value = player.energy;
        armorIcon.visible = player.armor;

        bossHealth = root.Q<ProgressBar>("BossHp");
        // bossHealthElement = root.Q<VisualElement>("boss-hp");
        if (gameManager.bossFight)
        {
            bossHealth.visible = true;
            // bossHealth.value = gameManager.bossEnemy.hp / gameManager.bossEnemy.maxHp * bossHealth.highValue;
            bossHealth.value = gameManager.bossEnemy.hp;
        }
        else
        {
            bossHealth.visible = false;
        }

    }
}
