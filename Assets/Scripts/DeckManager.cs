using System.Collections.Generic;
using UnityEngine;

public static class DeckManager
{
    public static GameObject HandZone;

    //Deck of cards (Unaffected by battle)
    public static List<Card> SetDeck = new List<Card>();

    //Deck of cards (For battle use)
    public static List<Card> Deck = new List<Card>();

    //Hand of cards
    public static List<Card> Hand = new List<Card>();
    public static List<GameObject> HandCards = new List<GameObject>();

    //Selected cards
    public static List<Card> SelectedCards = new List<Card>();
    public static List<GameObject> SelectedPhysicalCards = new List<GameObject>();

    //Discarded cards
    public static List<Card> Discards = new List<Card>();

}