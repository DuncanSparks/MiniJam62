using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [System.Serializable]
	class DialogueSetArray
	{
		public string[] array;
	}

    [SerializeField]
    string npcName;

    [SerializeField]
    DialogueSetArray[] dialogue;

    int dialogueSet = 0;
    public int DialogueSet { get => dialogueSet; set => dialogueSet = value; }

    bool playerInRange = false;

    void Update()
    {
        if (Input.GetButtonDown("Action") && playerInRange && !Controller.Singleton.player.GetComponent<Player>().LockMovement)
        {
            Controller.Singleton.player.GetComponent<Player>().LockMovement = true;
            Controller.Singleton.Dialogue(dialogue[dialogueSet].array, this);
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

    public int NumSets { get => dialogue.Length; }
}
