using System.Collections.Generic;
using UnityEngine;

public static class DeckManager
{
    public static GameObject HandZone;
    public static List<Card> Deck = new List<Card>();
    
    public static List<Card> Hand = new List<Card>();
    public static List<GameObject> HandCards = new List<GameObject>();

    public static List<Card> SelectedCards = new List<Card>();
    public static List<GameObject> SelectedPhysicalCards = new List<GameObject>();

    public static List<Card> Discards = new List<Card>();
}
