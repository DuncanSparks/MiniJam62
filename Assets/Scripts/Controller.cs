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

	Dictionary<string, bool> clearedScenes = new Dictionary<string, bool>(){};

    // ======================================================

	[SerializeField]
	float masterVolume = 1f;

    [SerializeField]
    GameObject dialogueObj = null;

    [SerializeField]
    GameObject comicTextObj = null;

    public GameObject player = null;

    string targetScene;
    string targetLocationObject = string.Empty;
    Vector3 targetScenePosition;
    Quaternion targetSceneRotation;

	AudioSource music;
	bool musicStarted = false;

    [SerializeField]
    bool onTitleScreen = false;

	int playerHealth = 4;
	public int PlayerHealth { set => playerHealth = value; }
	bool healPlayer = false;

    // ======================================================

    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
		AudioListener.volume = masterVolume;

        if (!onTitleScreen)
        {
            targetScenePosition = player.transform.position;
            targetSceneRotation = player.transform.rotation;
            Cursor.lockState = CursorLockMode.Locked;
        }

		music = GetComponents<AudioSource>()[8];
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Cursor.lockState = CursorLockMode.None;

		if (Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.F11))
		{
			Screen.fullScreenMode = Screen.fullScreenMode == FullScreenMode.Windowed ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
			//Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
		}

		if (musicStarted && !music.isPlaying)
		{
			music.time = 7.626f;
			music.Play();
		}
	}

	public void PlayMusic(AudioClip mus)
	{
		music.clip = mus;
		music.Play();
	}

	public void StopMusic()
	{
		music.Stop();
	}

	public void AddClearedScene(string name)
	{
		clearedScenes.Add(name, true);
	}

	public bool IsSceneCleared(string name)
	{
		return clearedScenes.ContainsKey(name);
	}

    public void Dialogue(string[] text, float pitch, NPC host)
    {
        if (dialogueOpen)
            return;

        var dlg = Instantiate(dialogueObj, Vector3.zero, Quaternion.identity);
        Dialogue dlgScript = dlg.GetComponent<Dialogue>();
		dlgScript.SoundPitch = pitch;
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

    public void Respawn()
    {
		ChangeScene(SceneManager.GetActiveScene().name, targetLocationObject);
		healPlayer = true;
    }

    public void ChangeScene(string scene, string locationObject)
    {
        if (!onTitleScreen)
        {
			var pl = player.GetComponent<Player>();
            pl.LockMovement = true;
        }
        
        GameUI.Singleton.Transition(true, playSound: true);
        targetLocationObject = locationObject;
        targetScene = scene;
        Invoke(nameof(ChangeScene2), 0.8f);
    }

    void ChangeScene2()
    {
        SceneManager.LoadScene(targetScene);
        Invoke(nameof(ChangeScene3), 0.02f);
    }

    void ChangeScene3()
    {
		if (targetLocationObject != string.Empty)
		{
			GameObject obj = GameObject.Find(targetLocationObject);
        	targetScenePosition = obj.transform.position;
       		targetSceneRotation = obj.transform.rotation;
		}
        
        player = FindObjectOfType<Player>().gameObject;
        var pl = player.GetComponent<Player>();
        player.transform.position = targetScenePosition;
        player.transform.rotation = targetSceneRotation;
        pl.ModelRotation = targetSceneRotation;
        pl.AimRotation = targetSceneRotation;
		if (healPlayer)
		{
			playerHealth = 4;
			pl.Health = 4;
			healPlayer = false;
		}


		GameUI.Singleton.SetHealth(playerHealth, 4);
        GameUI.Singleton.Transition(false, playSound: true);
        pl.GetComponent<Player>().LockMovement = true;
        pl.GetComponent<Player>().CurrentColor = GameUI.Singleton.CurrentColor;
        pl.UpdateColorInfo();
        Invoke(nameof(ChangeScene4), 0.4f);
    }

    void ChangeScene4()
    {
		var pl = player.GetComponent<Player>();
        pl.LockMovement = false;
		if (pl.Respawning)
		{
			pl.Respawning = false;
		}

        if (onTitleScreen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            onTitleScreen = false;
        }
    }
}
