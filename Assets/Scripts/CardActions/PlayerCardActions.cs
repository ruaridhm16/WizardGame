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
        for (int i = 0; i < numCards; i++)
        {
            DrawCard(DeckManager.Deck[UnityEngine.Random.Range(0, DeckManager.Deck.Count - 1)]);
        }
    }

    public override void CastSelectedCards(BattleManager.CastTargets castTarget)
    {
        int cost = CalculateCastBindManaCost(DeckManager.SelectedCards);

        foreach (Card card in DeckManager.SelectedCards)
        {
            GameObject physicalCard = card.spawnedCard;

            card.OnCast(castTarget);

            DeckManager.Hand.Remove(card);
            DeckManager.HandCards.Remove(physicalCard);
            card.spawnedCard = null;
            Destroy(physicalCard);
        }
        DeckManager.SelectedCards.Clear();
        DeckManager.SelectedPhysicalCards.Clear();
        DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();

        PlayerValueManager.Mana -= cost;

        // Play card casting animation 


        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurnComplete = true;
    }

    public override void DiscardSelectedCards()
    {
        foreach (Card card in DeckManager.SelectedCards)
        {
            GameObject physicalCard = card.spawnedCard;

            DeckManager.Discards.Add(card);

            DeckManager.Hand.Remove(card);
            DeckManager.HandCards.Remove(physicalCard);
            card.spawnedCard = null;
            Destroy(physicalCard);
        }
        DeckManager.SelectedCards.Clear();
        DeckManager.SelectedPhysicalCards.Clear();
        DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();

        // Play card casting animation 


        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurnComplete = true;
    }

    public override void BindSelectedCards()
    {
        int cost = CalculateCastBindManaCost(DeckManager.SelectedCards);

        int numSelected = DeckManager.SelectedCards.Count;

        for (int i = 0; i < numSelected; i++)
        {
            Card card = DeckManager.SelectedCards[0];

            GameObject physicalCard = card.spawnedCard;
            GameObject targetParent = DeckManager.BoundSlots.Find(o => o.GetComponent<BindSlot>().occupied == false);

            Bind(card, targetParent);
            card.OnBind(true);

            DeckManager.SelectedPhysicalCards.Remove(physicalCard);
            DeckManager.SelectedCards.Remove(card);
        }
        DeckManager.SelectedCards.Clear();
        DeckManager.SelectedPhysicalCards.Clear();
        DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();

        PlayerValueManager.Mana -= cost;

        // Play card binding animation 

        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurnComplete = true;
    }

    public override void Bind(Card card, GameObject destination)
    {
        GameObject physicalCard = card.spawnedCard;

        physicalCard.transform.parent = destination.transform;
        physicalCard.transform.localPosition = Vector3.zero;
        physicalCard.transform.localScale = Vector3.one;
        //physicalCard.GetComponent<CardView>().Flip();
        destination.GetComponent<BindSlot>().occupied = true;
        destination.GetComponent<BindSlot>().boundCard = card;

        physicalCard.GetComponent<CardInteractions>().Deselect();
        physicalCard.GetComponent<CardInteractions>().isInteractible = false;

        DeckManager.Hand.Remove(card);
        DeckManager.HandCards.Remove(physicalCard);
    }
    
    public override void SummonCards(int space, int cardsInDeck)
    {
        if (space > 1 && cardsInDeck >= 2)
        {
            DrawNumCards(2);
            PlayerValueManager.Mana -= 2;
        }
        else if (space == 1 || cardsInDeck == 1)
        {
            DrawNumCards(1);
            PlayerValueManager.Mana -= 2;
        }

        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurnComplete = true;
    }
}
