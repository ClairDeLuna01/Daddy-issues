using System.Linq;
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

        var actualBar = root.Query<ProgressBar>("energy-bar").Children<VisualElement>("unity-progress-bar").First();
        actualBar.style.backgroundColor = Color.cyan;

        var barBackground = root.Query<ProgressBar>("energy-bar").Children<Label>().First();
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
            bossHealth.value = gameManager.bossEnemy.hp;
        }
        else
        {
            bossHealth.visible = false;
        }
    }
}
