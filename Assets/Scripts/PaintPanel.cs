using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPanel : MonoBehaviour
{
	[SerializeField]
	bool anyColor = false;
    [SerializeField]
    bool displayTargetColor = false;
    [SerializeField]
    bool displayOnly = false;
    bool lastDipslayOnly = false;

	[SerializeField]
	Player.PaintColor requiredAnyColor;
	Player.PaintColor lastRequiredAnyColor;

	public Player.PaintColor RequiredAnyColor { get => requiredAnyColor; }

	[Space(50)]
	[SerializeField]
	AudioClip paintSound;

	[SerializeField]
	Color[] colors;

	[SerializeField]
	Material[] colorMaterials;

	int currentColor = -1;
	public int CurrentColor { get => currentColor; }

	bool painted = false;

    SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        if (!displayTargetColor)
        {
            Color color =  anyColor ? Color.white : colors[(int)requiredAnyColor];
            SetChildColors(color);
        }
    }

    void SetChildColors(Color color) {
        foreach (var sprite in sprites)
        {
            sprite.color = color;
        }
    }

    void UpdateChildColors()
    {
        Color color = colors[(int)requiredAnyColor];
        if(color == null)return;
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = color;
        }
        if(displayOnly)
        {
			MeshRenderer mesh = GetComponent<MeshRenderer>();
            mesh.material = colorMaterials[(int)requiredAnyColor];
        }
    }

    void OnValidate() {
        if(lastRequiredAnyColor != requiredAnyColor)
        {
            UpdateChildColors();
        }
        if(lastDipslayOnly != displayOnly)
        {
            UpdateChildColors();
        }
        lastRequiredAnyColor = requiredAnyColor;
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
                    Color color = color = colors[colorIndex];
                    int requiredIndex = (int)requiredAnyColor;
                    // color = colorIndex == requiredIndex ? Color.clear : colors[requiredIndex];
                    foreach (var sprite in sprites)
                    {
                        sprite.gameObject.SetActive(requiredIndex != colorIndex);
                    }
                    if(!displayTargetColor)
                    {
                        SetChildColors(color);
                    }
					currentColor = colorIndex;
				}
			}
			else if (!painted && collider.gameObject.GetComponentInParent<PaintGlob>().Color == requiredAnyColor)
			{
				Controller.Singleton.PlaySoundOneShot(paintSound, Random.Range(0.9f, 1.1f), 0.6f);
				mesh.material = colorMaterials[(int)requiredAnyColor];
				painted = true;
                foreach (var sprite in sprites)
                {
                    sprite.gameObject.SetActive(false);
                }
			}
		}
	}
}
