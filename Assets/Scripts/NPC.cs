using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    const float RotationInterpolateSpeed = 10.0f;

    [System.Serializable]
	class DialogueSetArray
	{
        [TextArea]
		public string[] array;
	}

    [SerializeField]
    string npcName;

    [SerializeField]
    DialogueSetArray[] dialogue;

    int dialogueSet = 0;
    public int DialogueSet { get => dialogueSet; set => dialogueSet = value; }

    bool playerInRange = false;

    [SerializeField]
    Transform model;
    [SerializeField]
    float maxAngle = -1;
    Quaternion startRotation;
    Quaternion targetRotation;

    void Start()
    {
        if (!model) model = transform.GetChild(0);
        startRotation = model.transform.rotation;
    }

    void Update()
    {
        if (Input.GetButtonDown("Action") && playerInRange && !Controller.Singleton.player.GetComponent<Player>().LockMovement)
        {
            Controller.Singleton.player.GetComponent<Player>().LockMovement = true;
            Controller.Singleton.Dialogue(dialogue[dialogueSet].array, this);
        }
        targetRotation = startRotation;
        if (playerInRange)
        {
            Vector3 diff = Controller.Singleton.player.transform.position - transform.position;
            diff.y=0;
            Quaternion lookRotation = Quaternion.LookRotation(diff);
            float angle = Quaternion.Angle(startRotation, lookRotation);
            if(angle<maxAngle)
            {
                targetRotation = lookRotation;
            }
        }
        model.rotation = Quaternion.Slerp(model.rotation, targetRotation, RotationInterpolateSpeed*Time.deltaTime);
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
