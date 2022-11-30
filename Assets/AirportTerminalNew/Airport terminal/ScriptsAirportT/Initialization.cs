using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{
	public GameObject infoPanel;
	public GameObject infoDialogueCanvas;
	public GameObject ADialogueCanvas;
	public GameObject HMDialogueCanvas;
	public GameObject BagInofCanvas;

	public GameObject HostMomNPC;

	public GameObject ArrowA;
	public GameObject ArrowHM;


	public GameObject Bag;

	private void Start()
    {
		BagInofCanvas.SetActive(false);
		infoPanel.SetActive(false);
		infoDialogueCanvas.SetActive(false);
		ADialogueCanvas.SetActive(false);
		HMDialogueCanvas.SetActive(false);

		HostMomNPC.SetActive(false);

		ArrowA.SetActive(false);

		Bag.SetActive(false);
	}
}
