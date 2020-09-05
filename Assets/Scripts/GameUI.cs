using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
	static GameUI singleton = null;
	public static GameUI Singleton { get => singleton; set => singleton = value; }

	[SerializeField]
	AudioClip colorSound;

	[SerializeField]
	AudioClip modeSound;

	[SerializeField]
	GameObject healthbar;

	[SerializeField]
	GameObject colorIndicator;

	[SerializeField]
	GameObject colorText;

	[SerializeField]
	GameObject aimModeText;

	[SerializeField]
	Color[] colors;

	readonly string[] colorNames = {
		"Red",
		"Blue",
		"Yellow"
	};

	void Awake()
	{
		if (Singleton == null)
		{
			Singleton = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SetHealth(int health, int maxhealth)
	{
		healthbar.GetComponent<Image>().fillAmount = (float)health / (float)maxhealth;
	}

    public void SetIndicatorColor(Player.PaintColor color)
	{
		Controller.Singleton.PlaySoundOneShot(colorSound, 1f, 0.4f);

		colorIndicator.GetComponent<Image>().color = colors[(int)color];
		var text = colorText.GetComponent<TextMeshProUGUI>();
		text.color = colors[(int)color];
		text.text = colorNames[(int)color];
	}

	public void SetAimMode(bool mouse)
	{
		Controller.Singleton.PlaySoundOneShot(modeSound);
		aimModeText.GetComponent<TextMeshProUGUI>().text = $"Aim mode: {(mouse ? "Mouse" : "Direction")}";
	}
}
