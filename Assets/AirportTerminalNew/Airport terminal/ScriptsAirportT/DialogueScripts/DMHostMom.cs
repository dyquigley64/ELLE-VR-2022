using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DMHostMom : MonoBehaviour
{
	public GameObject HMDialogueCanvas;
	public GameObject infoDialogueCanvas;
	public GameObject HMTrigger;

	public Text HMName;
	public Text HMDialogueText;
	public TextMeshProUGUI phoneInstructionsRight, phoneInstructionsLeft;

	public GameObject phoneRight, phoneLeft;
	public GameObject startBTN;
	public GameObject nextBTN;
	public GameObject exitBTN;

	public GameObject bag;
	public GameObject depositPoint;

	private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		startBTN.SetActive(false);
		nextBTN.SetActive(true);

		HMName.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			string nextInstructions = "Find your bag and bring it to your host parent";
			EndDialogue();
			//HMDialogueCanvas.SetActive(false);
			nextBTN.SetActive(false);
			//infoDialogueCanvas.SetActive(true);
			HMTrigger.SetActive(false);
			bag.SetActive(true);
			depositPoint.SetActive(true);
			if (phoneLeft.activeSelf)
				phoneInstructionsLeft.SetText(nextInstructions);
			else
				phoneInstructionsRight.SetText(nextInstructions);
			return;
		}


		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	public void DisplayFinalSentence()
	{
		string sentence = "Great, you found it! Ready to go?";
		StartCoroutine(TypeSentence(sentence));
		exitBTN.SetActive(true);
		depositPoint.SetActive(false);
	}

	IEnumerator TypeSentence(string sentence)
	{
		HMDialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			HMDialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		Debug.Log("end of convo");
	}
}
