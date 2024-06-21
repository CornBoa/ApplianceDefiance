using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class MonologueTrigger : MonoBehaviour
{
	public Monologue dialogue;
	public UnityEvent OnEnd;
	public void TriggerDialogue ()
	{
        FindObjectOfType<DialogueManager>().StartMonologue(dialogue,this);
    }
}
