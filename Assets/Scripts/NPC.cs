using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [System.Serializable]
	class DialogueSet
	{
		public string[] array;
	}

    [SerializeField]
    string npcName;

    [SerializeField]
    DialogueSet[] dialogue;

    int dialogueSet = 0;

    bool playerInRange = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Action") && playerInRange)
        {
            Controller.Singleton.Dialogue(dialogue[dialogueSet].array);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
