using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("InitialDeck")]
    public List<Card> InitialDeck;
    public GameObject HandZone;
    public GameObject cardPrefab;
    private HandManager HandManager;

    void Start()
    {
        HandManager = HandZone.GetComponent<HandManager>();
        DeckManager.HandZone = HandZone;

        foreach (Card card in InitialDeck)
        {
            DeckManager.Deck.Add(card);
        }

        StartCoroutine(HandManager.SpawnCards(cardPrefab, InitialDeck.Count));

    }

    
}
