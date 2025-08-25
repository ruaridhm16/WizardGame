using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCardActions : CardActions
{
    public override IEnumerator DrawInitialHand()
    {
        yield return new WaitForSeconds(1);

        handZone = DeckManager.HandZone;
        handManager = handZone.GetComponent<HandManager>();
        handManager.ResizeBounds(PlayerValueManager.handDrawSize);

        for (int i = 0; i < PlayerValueManager.handDrawSize; i++)
        {
            DrawCard(DeckManager.Deck[UnityEngine.Random.Range(0, DeckManager.Deck.Count - 1)]);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void DrawCard(Card card)
    {
        BoxCollider2D handZoneCollider = handZone.GetComponent<BoxCollider2D>();
        GameObject physicalCard = Instantiate(cardPrefab, position: new Vector2(100, handZoneCollider.bounds.center.y), rotation: quaternion.identity, parent: DeckManager.HandZone.transform);

        physicalCard.GetComponent<CardView>().SetCard(card);

        DeckManager.Hand.Add(card);
        DeckManager.HandCards.Add(physicalCard);
        card.spawnedCard = physicalCard;
        DeckManager.Deck.Remove(card);

        SpriteRenderer sr = physicalCard.GetComponent<SpriteRenderer>();
        sr.sprite = card.cardFace;
        sr.enabled = true;
        handManager.UpdateHandView();
    }
    public override void DrawNumCards(int numCards)
    {
        for (int i = 0; i < numCards; i++) {
            DrawCard(DeckManager.Deck[UnityEngine.Random.Range(0, DeckManager.Deck.Count - 1)]);
        }
    }
}
