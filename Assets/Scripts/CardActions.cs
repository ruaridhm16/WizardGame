using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CardActions : MonoBehaviour
{
    public GameObject cardPrefab;
    private HandManager HandManager;
    public IEnumerator DrawInitialHand(int numCards)
    {
        yield return new WaitForSeconds(2);

        HandManager = DeckManager.HandZone.GetComponent<HandManager>();
        HandManager.ResizeBounds(numCards);

        

        for (int i = 0; i < numCards; i++)
        {
            DrawCard(DeckManager.Deck[UnityEngine.Random.Range(0, DeckManager.Deck.Count - 1)]);

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void DrawCard(Card card)
    {
        BoxCollider2D handZoneCollider = DeckManager.HandZone.GetComponent<BoxCollider2D>();
        GameObject physicalCard = Instantiate(cardPrefab, position: new Vector2(100, handZoneCollider.bounds.center.y), rotation: quaternion.identity, parent: DeckManager.HandZone.transform);

        physicalCard.GetComponent<CardView>().SetCard(card);

        DeckManager.Hand.Add(card);
        DeckManager.HandCards.Add(physicalCard);
        card.SpawnedPhysicalCard = physicalCard;
        DeckManager.Deck.Remove(card);

        SpriteRenderer sr = physicalCard.GetComponent<SpriteRenderer>();
        sr.sprite = card.cardFace;
        sr.enabled = true;
        HandManager.UpdateHandView();
    }

    public void DrawNumCards(int numCards)
    {
        for (int i = 0; i < numCards; i++) {
            DrawCard(DeckManager.Deck[UnityEngine.Random.Range(0, DeckManager.Deck.Count - 1)]);
        }
    }

    public void DestroyRandomCards()
    {

    }

    public void CastSelectedCards()
    {
        int total = 0;
        foreach (Card card in DeckManager.SelectedCards)
        {
            total += card.manaCost;
        }


        foreach (GameObject physicalCard in DeckManager.SelectedPhysicalCards)
        {
            Card card = physicalCard.GetComponent<CardView>().card;

            DeckManager.Hand.Remove(card);
            DeckManager.HandCards.Remove(physicalCard);
            Destroy(physicalCard);
        }
        DeckManager.SelectedCards.Clear();
        DeckManager.SelectedPhysicalCards.Clear();
        DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();

        PlayerValueManager.Mana -= total;
    }

    public void BindSelectedCards()
    {
        
    }
}
