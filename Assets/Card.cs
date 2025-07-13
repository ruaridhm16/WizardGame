using System;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [HideInInspector] public string cardName = "fesfefsef";
    [HideInInspector] public int manaCost;
    public Transform cardPrefab;

    public abstract void CardEffect();

    public abstract void OnCardSelect();

    public abstract void OnCardDrag();

    public abstract void OnCardDrop();
    public abstract void OnCardEntry();
    public abstract void OnCardDeletion();
    public abstract void CardPassiveEffect();
}
