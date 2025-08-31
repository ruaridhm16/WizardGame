using UnityEngine;
using System.Collections.Generic;

public abstract class CardData : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    public string cardDescription;
    public string bindDescription;
    public string castDescription;
    public int baseCardHealth = 10;
    [HideInInspector] public bool isPlayerCard;

    public List<Card.CardAttribute> BaseCardAttributes = new List<Card.CardAttribute>();

    public abstract Card CreateInstance(BattleManager BattleManager);

    




}