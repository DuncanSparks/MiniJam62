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
	Sprite[] colorLetters;

	[SerializeField]
	Material[] colorMaterials;

	[SerializeField]
	Color[] letterColors;

	int currentColor = -1;
	public int CurrentColor { get => currentColor; }

	bool painted = false;

	bool canBePainted = true;
	public bool CanBePainted { set => canBePainted = value; }

    SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
		sprites[0].sprite = colorLetters[(int)requiredAnyColor];
        if (!displayTargetColor)
        {
            Color color =  anyColor ? Color.white : colors[(int)requiredAnyColor];
            SetChildColors(color);
        }
    }

	void Update()
	{
		if (!Controller.Singleton.ColorblindMode)
		{
			sprites[0].gameObject.SetActive(false);
			return;
		}

		sprites[0].color = displayOnly || (displayTargetColor && currentColor != -1) || (!displayTargetColor) ? letterColors[1] : letterColors[0];
		
		if (displayTargetColor || displayOnly)
		{
			sprites[0].gameObject.SetActive(currentColor != (int)requiredAnyColor);
		}
		else if (currentColor != -1)
		{
			sprites[0].gameObject.SetActive(true);
			sprites[0].sprite = colorLetters[currentColor];
		}
		else
		{
			sprites[0].gameObject.SetActive(false);
		}
	}

    void SetChildColors(Color color) {
		for (int i = 1; i < sprites.Length; i++)
		{
			sprites[i].color = color;
		}
    }

    void UpdateChildColors()
    {
        Color color = colors[(int)requiredAnyColor];
        if(color == null)return;

		var _sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 1; i < _sprites.Length; i++)
        {
            _sprites[i].color = color;
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
		if (canBePainted)
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

					for (int i = 1; i < sprites.Length; i++)
					{
						sprites[i].gameObject.SetActive(requiredIndex != colorIndex);
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

	public void Solve()
	{
		var _sprites = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 1; i < _sprites.Length; i++)
		{
			_sprites[i].gameObject.SetActive(false);
		}

		MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.material = colorMaterials[(int)requiredAnyColor];
		currentColor = (int)requiredAnyColor;
	}
}
