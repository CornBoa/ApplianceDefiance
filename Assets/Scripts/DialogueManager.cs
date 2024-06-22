using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public class DialogueManager : MonoBehaviour 
{	
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	public GameObject DialoguePanel;
	private Queue<string> sentences;
	List<Sentence> sentenceList;
	int sentenceIndex = 0;
	public UnityEvent OnDIalogueEnd;
	public Image Portrait,PortraitLeft;
	DialogueTrigger CUrrentDialogue;
	public static bool DialogueON;
	public static bool UnlockMovement;
	public GameObject RegUI;
	void Start () {
		UnlockMovement = true;
		sentences = new Queue<string>();
        Cursor.lockState = CursorLockMode.Confined;
    }
	public void StartMonologue (Dialogue dialogue,DialogueTrigger dialogueTrigger)
	{
		RegUI.SetActive(false);
		sentenceList = dialogue.sentences.ToList();		
		Portrait.sprite = sentenceList[sentenceIndex].portrait;
		Time.timeScale = 0;
		CUrrentDialogue = dialogueTrigger;
		DialogueON = true;
		DialoguePanel.SetActive(true);		
		sentences.Clear();
		foreach (Sentence sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence.ToString());
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
        if (sentenceList[sentenceIndex].rightLeftSide)
        {
            Portrait.gameObject.SetActive(false);
            PortraitLeft.gameObject.SetActive(true);
            PortraitLeft.sprite = sentenceList[sentenceIndex].portrait;
        }
		else 
		{
            Portrait.gameObject.SetActive(true);
            PortraitLeft.gameObject.SetActive(false);
            Portrait.sprite = sentenceList[sentenceIndex].portrait;
        }
        sentenceIndex++;
        string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));

    }
	IEnumerator TypeSentence(string sentence)
	{
		nameText.text = sentence.Split("!:")[0];
		dialogueText.text = "";
		List<string> strings = sentence.Split().ToList();		
		strings.RemoveAt(0);
		string sentenceToUse = string.Join(" ",strings.ToArray());
        foreach (char letter in sentenceToUse.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue(DialogueTrigger dialogueTrigger)
	{
        RegUI.SetActive(true);
        Time.timeScale = 1;
        DialogueON = false;
		DialoguePanel.SetActive (false);
		dialogueTrigger.OnEnd.Invoke();
    }

}
