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

    void Start()
    {



        foreach (Card card in InitialDeck)
        {
            DeckManager.Deck.Add(card);
            print(card.cardName + " added to deck");
        }

        StartCoroutine(CardSpawn());

        print(DeckManager.Hand);

    }

    public IEnumerator CardSpawn()
    {
        BoxCollider2D handZoneCollider = HandZone.GetComponent<BoxCollider2D>();

        for (int i = 0; i < PlayerValueManager.HandSize; i++)
        {
            Card spawnedCard = DeckManager.Deck[0];


            GameObject currentCard = Instantiate(cardPrefab, position: new Vector2(handZoneCollider.bounds.min.x + (handZoneCollider.bounds.size.x / PlayerValueManager.HandSize) * (i + 0.5f), handZoneCollider.bounds.center.y), rotation: quaternion.identity);
            currentCard.GetComponent<CardView>().SetCard(spawnedCard);

            DeckManager.Hand.Add(spawnedCard);
            DeckManager.Deck.Remove(spawnedCard);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
