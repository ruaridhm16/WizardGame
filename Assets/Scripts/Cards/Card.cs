using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    public bool flipped = false;

    [HideInInspector] public GameObject SpawnedPhysicalCard;
    public abstract void OnCast();
    public virtual void OnDraw() { }
    public virtual void OnDiscard() { }

    




}