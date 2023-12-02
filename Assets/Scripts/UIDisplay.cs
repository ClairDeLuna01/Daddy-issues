using UnityEngine;
using UnityEngine.UIElements;

public class UIDisplay : MonoBehaviour
{
	private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
		VisualElement root = GetComponent<UIDocument>().rootVisualElement;
		Label armorIcon = root.Q<Label>("armor-icon");
		ProgressBar energyBar = root.Q<ProgressBar>("energy-bar");

		energyBar.value = player.energy;
		armorIcon.visible = player.armor;
        
    }
}
