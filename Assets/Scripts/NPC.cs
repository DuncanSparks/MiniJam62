﻿using System.Collections;
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

	[Range(0, 2)]
	[SerializeField]
	float textSoundPitch = 1f;

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

	SpriteRenderer interactIndicator;

	[SerializeField]
	bool isTransition = false;

	[SerializeField]
	Object targetScene;
    [SerializeField]
    string targetSceneName;

	[SerializeField]
	string targetLocationObject;

	[Space(20)]
    [SerializeField]
    Event dialogueEndEvent;

    void OnValidate() {
      if (targetScene)
        targetSceneName = targetScene.name;
    }

    void Start()
    {
        interactIndicator = GetComponentInChildren<SpriteRenderer>();
        interactIndicator.enabled = false;

        if (model)
        startRotation = model.transform.rotation;
    }

    void Update()
    {
        if (Input.GetButtonDown("Action") && playerInRange && !Controller.Singleton.player.GetComponent<Player>().LockMovement)
        {
			if (!isTransition)
			{
				interactIndicator.enabled = false;
            	Controller.Singleton.player.GetComponent<Player>().LockMovement = true;
            	Controller.Singleton.Dialogue(dialogue[dialogueSet].array, textSoundPitch, this);
			}
			else
			{
				Controller.Singleton.ChangeScene(targetSceneName, targetLocationObject);
			}
        }

        if (model)
        {
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
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
			interactIndicator.enabled = true;
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
			interactIndicator.enabled = false;
            playerInRange = false;
        }
    }

	public void EndDialogue()
	{
		dialogueSet = Mathf.Min(++dialogueSet, dialogue.Length - 1);
		interactIndicator.enabled = true;
		dialogueEndEvent.Invoke();
	}

    //public int NumSets { get => dialogue.Length; }
}
