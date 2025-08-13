using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;

    [HideInInspector] public GameObject SpawnedPhysicalCard;
    public abstract void OnActivation();
    public virtual void OnDraw() { }
    public virtual void OnDiscard() { }

    

    
}