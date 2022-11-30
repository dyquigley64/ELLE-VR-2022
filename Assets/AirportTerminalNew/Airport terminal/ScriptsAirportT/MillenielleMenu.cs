using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillenielleMenu : MonoBehaviour
{
	public GameObject infoPanel;
	public GameObject StartBTN;
	public GameObject SkipBTN;

	public void StartTutorialClicked()
	{
		StartBTN.SetActive(false);
		SkipBTN.SetActive(false);
		infoPanel.SetActive(true);

	}
}
