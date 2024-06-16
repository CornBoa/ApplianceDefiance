using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{	
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	public GameObject DialoguePanel;
	private Queue<string> sentences;
	List<Sprite> dialoguePortraits;
	public UnityEvent OnDIalogueEnd;
	public Image Portrait;
	MonologueTrigger CUrrentDialogue;
	public static bool DialogueON;
	public static bool UnlockMovement;
	public int portraitIndex = 0;
	public GameObject RegUI;
	void Start () {
		UnlockMovement = true;
		sentences = new Queue<string>();
        Cursor.lockState = CursorLockMode.Confined;
    }
	public void StartMonologue (Dialogue dialogue,MonologueTrigger dialogueTrigger)
	{
		RegUI.SetActive(false);
		dialoguePortraits = dialogue.portraitsOrder;
		Portrait.sprite = dialoguePortraits[portraitIndex];
		Time.timeScale = 0;
		CUrrentDialogue = dialogueTrigger;
		DialogueON = true;
		DialoguePanel.SetActive(true);
		nameText.text = dialogue.name;
		sentences.Clear();
		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		DisplayNextSentenceMono();
	}

	public void DisplayNextSentenceMono ()
	{		
		Debug.Log("NextDialogue");
		if (sentences.Count == 0)
		{
			EndDialogue(CUrrentDialogue);
			return;
		}		
        Portrait.sprite = dialoguePortraits[portraitIndex];
        portraitIndex++;
        string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue(MonologueTrigger dialogueTrigger)
	{
        RegUI.SetActive(true);
        Time.timeScale = 1;
        DialogueON = false;
		DialoguePanel.SetActive (false);
		dialogueTrigger.OnEnd.Invoke();
    }

}
