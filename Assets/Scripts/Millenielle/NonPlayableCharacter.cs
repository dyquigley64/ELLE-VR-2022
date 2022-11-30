using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NonPlayableCharacter", menuName="ScriptableObjects/NPC", order =1)]
public class NonPlayableCharacter : ScriptableObject
{
    // Descriptors for the npc
    public enum Descriptor {M, F};
    public enum Top {Shirt, Suit, Sweater};
    public enum Color {Blue, Gray, White, Black, Striped, Red};
    public Color shirtColor;
    public Top top;
    public Descriptor playerDescriptor;
    public Descriptor topDescriptor;
    public GameObject model;
}

