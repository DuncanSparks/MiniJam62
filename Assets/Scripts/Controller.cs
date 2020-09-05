using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	bool destroyThis = false;

	static Controller singleton = null;
	public static Controller Singleton { get => singleton; }

	bool dialogueOpen = false;
	public bool DialogueOpen { get => dialogueOpen; set => dialogueOpen = value; }

	int audioSourceIndex = 0;

	// ======================================================

	[SerializeField]
	GameObject dialogueObj = null;

	[SerializeField]
	GameObject comicTextObj = null;

	public GameObject player = null;

	// ======================================================

	void Awake()
	{
		if (singleton == null)
			singleton = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void Dialogue(string[] text, NPC host)
	{
		if (dialogueOpen)
			return;

		var dlg = Instantiate(dialogueObj, Vector3.zero, Quaternion.identity);
		Dialogue dlgScript = dlg.GetComponent<Dialogue>();
		dlgScript.Host = host;
		dlgScript.DialogueText = text;
		dlgScript.StartDialogue();
		dialogueOpen = true;
	}

	public void PlaySoundOneShot(AudioClip sound, float pitch = 1f, float volume = 1f)
	{
		AudioSource source = GetComponents<AudioSource>()[audioSourceIndex];
		source.clip = sound;
		source.pitch = pitch;
		source.volume = volume;
		source.Play();
		audioSourceIndex = (audioSourceIndex + 1) % 8;
	}

	public void ShowComicText(string text, Vector3 position, Camera camera)
	{
		var obj = Instantiate(comicTextObj, position, Quaternion.identity);
		obj.GetComponent<Canvas>().worldCamera = camera;
		obj.GetComponent<BillboardFX>().camTransform = camera.transform;
		obj.GetComponent<ComicText>().DisplayText(text);
		Destroy(obj, 0.333f);
	}
}
