using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTAssistant : MonoBehaviour
{
	public Dialogue dialogue;

	public void TriggerDialogue()
	{
		FindObjectOfType<DMAssistant>().StartDialogue(dialogue);
	}
}
