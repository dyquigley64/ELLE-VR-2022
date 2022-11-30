using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DMAssistant : MonoBehaviour
{
	public GameObject ADialogueCanvas;
	public GameObject infoDialogueCanvas;

	public Text AName;
	public Text ADialogueText;
	public TextMeshProUGUI phoneInstructionsRight, phoneInstructionsLeft;
	
	public GameObject phoneRight, phoneLeft;
	public GameObject startBTN;
	public GameObject nextBTN;
	//public GameObject choices;

	public GameObject HostMomNPC;

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

		AName.text = dialogue.name;

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
			string nextInstructions = "Greet your host parent in the seating area";
			EndDialogue();
			ADialogueCanvas.SetActive(false);
			infoDialogueCanvas.SetActive(true);
			HostMomNPC.SetActive(true);
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

	IEnumerator TypeSentence(string sentence)
	{
		ADialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			ADialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		Debug.Log("end of convo");
	}
}
