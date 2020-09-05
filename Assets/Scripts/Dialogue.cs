using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{	
	string[] dialogueText;
	int dialoguePage = 0;
	float visibleCharacters = 0;

	bool rollText = false;

	public string[] DialogueText { set => dialogueText = value; }

	TextMeshProUGUI text;
	Image textbox;
	
	void Start()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		textbox = GetComponentInChildren<Image>();
	}

	void Update()
	{
		if (rollText)
		{
			visibleCharacters = Mathf.Clamp(visibleCharacters + 25f * Time.deltaTime, 0, dialogueText[dialoguePage].Length);
			text.maxVisibleCharacters = Mathf.RoundToInt(visibleCharacters);

			if (Input.GetButtonDown("Action"))
			{
				if (visibleCharacters < dialogueText[dialoguePage].Length)
					visibleCharacters = dialogueText[dialoguePage].Length;
				else
				{
					if (dialoguePage < dialogueText.Length - 1)
					{
						visibleCharacters = 0;
						dialoguePage++;
						text.text = dialogueText[dialoguePage];
					}
					else
					{
						Controller.Singleton.DialogueOpen = false;
						Destroy(gameObject);
					}
				}
			}
		}
	}

	public void StartDialogue()
	{
		var txt = GetComponentInChildren<TextMeshProUGUI>();
		txt.maxVisibleCharacters = 0;
		txt.text = dialogueText[0];
		rollText = true;
	}
}
