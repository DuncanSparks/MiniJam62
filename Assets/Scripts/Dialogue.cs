using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{	
	List<string> dialogueText = new List<string>();
	int dialoguePage = 0;
	float visibleCharacters = 0;

	TextMeshProUGUI text;
	Image textbox;
	
	void Start()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		textbox = GetComponentInChildren<Image>();

		text.maxVisibleCharacters = 0;
		
		dialogueText.Add("Hello there");
		dialogueText.Add("This is a test");
		dialogueText.Add("How are you doing today friend");

		text.text = dialogueText[0];
	}

	void Update()
	{
		visibleCharacters = Mathf.Clamp(visibleCharacters + 25f * Time.deltaTime, 0, dialogueText[dialoguePage].Length);
		text.maxVisibleCharacters = Mathf.RoundToInt(visibleCharacters);

		if (Input.GetButtonDown("Action"))
		{
			if (visibleCharacters < dialogueText[dialoguePage].Length)
				visibleCharacters = dialogueText[dialoguePage].Length;
			else
			{
				if (dialoguePage < dialogueText.Count - 1)
				{
					visibleCharacters = 0;
					dialoguePage++;
				}
				else
				{
					
				}
			}
		}
	}
}
