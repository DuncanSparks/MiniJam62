using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	bool destroyThis = false;

	static Controller singleton = null;
	public static Controller Singleton { get => singleton; }
	Controller()
	{
		if (singleton == null)
			singleton = this;
		else
			destroyThis = true;
	}

	bool dialogueOpen = false;
	public bool DialogueOpen { get => dialogueOpen; set => dialogueOpen = value; }

	// ======================================================

	[SerializeField]
	GameObject dialogueObj = null;

	public GameObject player = null;

	// ======================================================

	void Awake()
	{
		if (destroyThis)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void Dialogue(List<string> text)
	{
		if (dialogueOpen)
			return;

		var dlg = Instantiate(dialogueObj, Vector3.zero, Quaternion.identity);
		Dialogue dlgScript = dlg.GetComponent<Dialogue>();
		dlgScript.DialogueText = text;
		dlgScript.StartDialogue();
		dialogueOpen = true;
	}
}
