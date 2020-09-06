using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
	bool destroyThis = false;

	static Controller singleton = null;
	public static Controller Singleton { get => singleton; set => singleton = value; }

	bool dialogueOpen = false;
	public bool DialogueOpen { get => dialogueOpen; set => dialogueOpen = value; }

	int audioSourceIndex = 0;

	// ======================================================

	[SerializeField]
	GameObject dialogueObj = null;

	[SerializeField]
	GameObject comicTextObj = null;

	public GameObject player = null;

	string targetScene;
	Vector3 targetScenePosition;
	Vector3 targetSceneRotation;

	// ======================================================

	void Awake()
	{
		if (Singleton == null)
			Singleton = this;
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
		Destroy(obj, 0.5f);
	}

	public void ChangeScene(string scene, Vector3 position, Vector3 rotation)
	{
		player.GetComponent<Player>().LockMovement = true;
		//GameUI.Singleton.Fade(Color.black, 1f, true);
		GameUI.Singleton.Transition(true);
		targetScene = scene;
		targetScenePosition = position;
		targetSceneRotation = rotation;
		Invoke(nameof(ChangeScene2), 0.8f);
	}

	void ChangeScene2()
	{
		SceneManager.LoadScene(targetScene);
		Invoke(nameof(ChangeScene3), 0.02f);
	}

	void ChangeScene3()
	{
		player = FindObjectOfType<Player>().gameObject;
		var pl = player.GetComponent<Player>();
		player.transform.position = targetScenePosition;
		Quaternion rot = Quaternion.Euler(targetSceneRotation);
		player.transform.rotation = rot;
		pl.ModelRotation = rot;
		pl.AimRotation = rot;
		//GameUI.Singleton.Fade(Color.black, 1f, false);
		GameUI.Singleton.Transition(false);
		pl.GetComponent<Player>().LockMovement = true;
		pl.GetComponent<Player>().CurrentColor = GameUI.Singleton.CurrentColor;
		pl.UpdateColorInfo();
		Invoke(nameof(ChangeScene4), 0.4f);
	}

	void ChangeScene4()
	{
		player.GetComponent<Player>().LockMovement = false;
	}
}
