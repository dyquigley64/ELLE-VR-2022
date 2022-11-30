using UnityEngine;

[System.Serializable]
public class LongQuestion
{
	public int questionID;
	public int audioID;
	public string audioLocation;
	public AudioClip audio;
	public int imageID;
	public string imageLocation;
	public Texture2D image;
	public string type;
	public string questionText;
	public Term[] answers;
}