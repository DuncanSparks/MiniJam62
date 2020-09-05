using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPanel : MonoBehaviour
{
	[SerializeField]
	Player.PaintColor panelColor;

	[SerializeField]
	AudioClip paintSound;

	[SerializeField]
	Color[] colors;

	[SerializeField]
	Material[] colorMaterials;

	readonly string[] colorNames = {
		"Red",
		"Blue",
		"Yellow"
	};

	bool painted = false;

    void Start()
    {
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
		{
			sprite.color = colors[(int)panelColor];
		}
    }

    void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "PlayerProjectile")
		{
			if (!painted && collider.gameObject.GetComponentInParent<PaintGlob>().Color == panelColor)
			{
				Controller.Singleton.PlaySoundOneShot(paintSound, Random.Range(0.9f, 1.1f));
				GetComponent<MeshRenderer>().material = colorMaterials[(int)panelColor];
				painted = true;
			}
		}
	}
}
