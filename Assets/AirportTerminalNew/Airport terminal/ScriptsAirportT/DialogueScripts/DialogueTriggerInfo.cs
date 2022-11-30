using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerInfo : MonoBehaviour
{
	public Dialogue dialogue;

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManagerInfo>().StartDialogue(dialogue);
	}
}
