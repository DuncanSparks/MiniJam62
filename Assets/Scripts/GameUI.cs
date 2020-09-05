using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	static GameUI singleton = null;
	public static GameUI Singleton { get => singleton; set => singleton = value; }

	[SerializeField]
	GameObject colorIndicator;

	readonly Color[] colors = {
		new Color(1, 0, 0),
		new Color(0, 0, 1),
		new Color(1, 1, 0)
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

    public void SetIndicatorColor(Player.PaintColor color)
	{
		colorIndicator.GetComponent<Image>().color = colors[(int)color];
	}
}
