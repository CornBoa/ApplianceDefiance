using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
	public UnityEvent OnEnd;
	public void TriggerDialogue ()
	{
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue,this);
    }

}
