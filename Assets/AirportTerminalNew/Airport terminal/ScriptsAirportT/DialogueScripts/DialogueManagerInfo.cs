using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManagerInfo : MonoBehaviour
{
	public GameObject infoDialogueCanvas;

	public Text infoDialogueText;
	public GameObject startBTN;
	public GameObject continueBTN;
	private Queue<string> sentences;




	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		startBTN.SetActive(false);
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()//all infomational sentences on different sections are stored in one Queue
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			infoDialogueCanvas.SetActive(false);
			return;
		}

		if (sentences.Count == 2)//canvas info shown after assistant dialogue is dequeued and hidden, waiting for when set to Active
		{
			string info1 = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(info1));
			infoDialogueCanvas.SetActive(false);
			return;
		}

		if (sentences.Count == 1)//info shown after host mom dialogue
		{
			string info2 = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(info2));
			infoDialogueCanvas.SetActive(false);
			
			return;
		}
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		infoDialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			infoDialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		Debug.Log("end of convo");
	}
}
