using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{	
	string[] dialogueText;
	public string[] DialogueText { set => dialogueText = value; }

	int dialoguePage = 0;
	float visibleCharacters = 0;
	int flooredVisibleCharacters = 0;

	bool rollText = false;

	NPC host = null;
	public NPC Host { set => host = value; }

	TextMeshProUGUI text;
	Image textbox;

	[SerializeField]
	AudioClip advanceSound;

	[SerializeField]
	AudioClip[] textSounds;

	float soundPitch = 1f;
	public float SoundPitch { set => soundPitch = value; }

	float pitchVariance = 0.02f;
	
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
			flooredVisibleCharacters = Mathf.FloorToInt(visibleCharacters);
			if (flooredVisibleCharacters % 2 == 0 && flooredVisibleCharacters < dialogueText[dialoguePage].Length)
			{
				Controller.Singleton.PlaySoundOneShot(textSounds[Random.Range(0, textSounds.Length)], soundPitch + Random.Range(-pitchVariance, pitchVariance), 0.05f);
			}

			if (Input.GetButtonDown("Action"))
			{
				if (visibleCharacters < dialogueText[dialoguePage].Length)
				{
					visibleCharacters = dialogueText[dialoguePage].Length;
				}
				else
				{
					if (dialoguePage < dialogueText.Length - 1)
					{
						//Controller.Singleton.PlaySoundOneShot(advanceSound, Random.Range(0.98f, 1.02f), 0.7f);
						visibleCharacters = 0;
						dialoguePage++;
						text.text = dialogueText[dialoguePage];
					}
					else
					{
						Controller.Singleton.DialogueOpen = false;
						Controller.Singleton.player.GetComponent<Player>().LockMovement = false;
						host.EndDialogue();
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
