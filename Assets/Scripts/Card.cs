using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    
    public abstract void OnActivation();
}
