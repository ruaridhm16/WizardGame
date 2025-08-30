using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    public string decription;
    public int baseCardHealth = 10;
    [HideInInspector] public bool isPlayerCard;

    public abstract Card CreateInstance(BattleManager BattleManager);

    




}