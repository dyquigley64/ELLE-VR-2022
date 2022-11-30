using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTHostMom : MonoBehaviour
{
	public Dialogue dialogue;

	public void TriggerDialogue()
	{
		FindObjectOfType<DMHostMom>().StartDialogue(dialogue);
	}
}
