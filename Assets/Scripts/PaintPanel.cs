using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPanel : MonoBehaviour
{
	[SerializeField]
	Player.PaintColor panelStartColor;

	[SerializeField]
	bool anyColor = false;

	[SerializeField]
	Player.PaintColor requiredAnyColor;

	[Space(50)]
	[SerializeField]
	AudioClip paintSound;

	[SerializeField]
	Color[] colors;

	[SerializeField]
	Material[] colorMaterials;

	int currentColor = -1;
	bool painted = false;

    void Start()
    {
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
		{
			sprite.color = anyColor ? Color.white : colors[(int)panelStartColor];
		}
    }

    void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "PlayerProjectile")
		{
			MeshRenderer mesh = GetComponent<MeshRenderer>();
			if (anyColor)
			{
				int colorIndex = (int)collider.gameObject.GetComponentInParent<PaintGlob>().Color;
				if (colorIndex != currentColor)
				{
					Controller.Singleton.PlaySoundOneShot(paintSound, Random.Range(0.9f, 1.1f), 0.6f);
					mesh.material = colorMaterials[colorIndex];
					foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
					{
						sprite.color = colors[colorIndex];
					}

					currentColor = colorIndex;
				}
			}
			else if (!painted && collider.gameObject.GetComponentInParent<PaintGlob>().Color == panelStartColor)
			{
				Controller.Singleton.PlaySoundOneShot(paintSound, Random.Range(0.9f, 1.1f), 0.6f);
				mesh.material = colorMaterials[(int)panelStartColor];
				painted = true;
			}
		}
	}
}
