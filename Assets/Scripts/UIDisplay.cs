using UnityEngine;
using UnityEngine.UIElements;

public class UIDisplay : MonoBehaviour
{
	private Player player;
	private VisualElement root;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
		root = GetComponent<UIDocument>().rootVisualElement;
		var actualBar = root.Q(className: "unity-progress-bar__progress");
        actualBar.style.backgroundColor = Color.cyan;
		var barBackground = root.Q(className: "unity-progress-bar__background");
		barBackground.style.backgroundColor = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
		Label armorIcon = root.Q<Label>("armor-icon");
		ProgressBar energyBar = root.Q<ProgressBar>("energy-bar");

		energyBar.value = player.energy;
		armorIcon.visible = player.armor;
        
    }
}
