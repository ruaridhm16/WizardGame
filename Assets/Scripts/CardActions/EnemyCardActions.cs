using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyCardActions : CardActions
{
    public EnemyManager enemyManager;

    public override IEnumerator DrawInitialHand()
    {
        yield return new WaitForSeconds(1);

        handZone = enemyManager.enemyHandZone;
        handManager = handZone.GetComponent<HandManager>();
        handManager.ResizeBounds(PlayerValueManager.handDrawSize);

        for (int i = 0; i < PlayerValueManager.handDrawSize; i++)
        {
            DrawCard(enemyManager.enemyDeck[UnityEngine.Random.Range(0, enemyManager.enemyDeck.Count - 1)]);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void DrawCard(Card card)
    {
        BoxCollider2D handZoneCollider = handZone.GetComponent<BoxCollider2D>();
        GameObject physicalCard = Instantiate(cardPrefab, position: new Vector2(100, handZoneCollider.bounds.center.y), rotation: quaternion.identity, parent: handZone.transform);

        physicalCard.GetComponent<CardView>().SetCard(card);

        enemyManager.enemyHand.Add(card);
        enemyManager.enemyHandCards.Add(physicalCard);
        card.spawnedCard = physicalCard;
        enemyManager.enemyDeck.Remove(card);

        SpriteRenderer sr = physicalCard.GetComponent<SpriteRenderer>();
        sr.sprite = card.cardFace;
        sr.enabled = true;
        physicalCard.transform.localScale = Vector2.one;

        physicalCard.GetComponent<CardInteractions>().isClickable = false;
        physicalCard.GetComponent<CardInteractions>().isDraggable = false;
        physicalCard.GetComponent<CardInteractions>().isTooltipable = false;

        card.isFlipped = true;
        card.isPlayerCard = false;
        handManager.UpdateHandView();
    }
    public override void DrawNumCards(int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            DrawCard(enemyManager.enemyDeck[UnityEngine.Random.Range(0, enemyManager.enemyDeck.Count - 1)]);
        }
    }

    public override void CastSelectedCards(BattleManager.CastTargets castTarget)
    {
        int cost = CalculateCastBindManaCost(enemyManager.enemySelectedCards);
        

        foreach (Card card in enemyManager.enemySelectedCards)
        {
            GameObject physicalCard = card.spawnedCard;

            card.OnCast(castTarget);

            enemyManager.enemyHand.Remove(card);
            enemyManager.enemyHandCards.Remove(physicalCard);
            card.spawnedCard = null;
            Destroy(physicalCard);
        }
        GetComponent<BattleManager>().lastCast = new List<Card>(enemyManager.enemySelectedCards);
        enemyManager.enemySelectedCards.Clear();
        enemyManager.enemySelectedPhysicalCards.Clear();
        enemyManager.enemyHandZone.GetComponent<HandManager>().UpdateHandView();

        enemyManager.mana -= cost;
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
        int cost = CalculateCastBindManaCost(enemyManager.enemySelectedCards);

        int numSelected = enemyManager.enemySelectedCards.Count;

        for (int i = 0; i < numSelected; i++)
        {
            Card card = enemyManager.enemySelectedCards[0];

            GameObject physicalCard = card.spawnedCard;
            GameObject targetParent = enemyManager.BoundSlots.Find(o => o.GetComponent<EnemyBindSlot>().occupied == false);

            Bind(card, targetParent);
            card.attatchedBindSlot = targetParent;
            card.OnBind(false);

            enemyManager.enemySelectedPhysicalCards.Remove(physicalCard);
            enemyManager.enemySelectedCards.Remove(card);
        }
        enemyManager.enemySelectedCards.Clear();
        enemyManager.enemySelectedPhysicalCards.Clear();
        enemyManager.enemyHandZone.GetComponent<HandManager>().UpdateHandView();

        enemyManager.mana -= cost;
    }

    public override void Bind(Card card, GameObject destination)
    {
        GameObject physicalCard = card.spawnedCard;

        physicalCard.transform.parent = destination.transform;
        physicalCard.transform.localPosition = Vector3.zero;
        physicalCard.transform.localScale = Vector3.one;
        //physicalCard.GetComponent<CardView>().Flip();
        destination.GetComponent<EnemyBindSlot>().occupied = true;
        destination.GetComponent<EnemyBindSlot>().boundCard = card;

        physicalCard.GetComponent<CardInteractions>().Deselect();
        physicalCard.GetComponent<CardInteractions>().isClickable = true;

        physicalCard.GetComponent<SpriteRenderer>().sortingOrder = 1;

        enemyManager.enemyHand.Remove(card);
        enemyManager.enemyHandCards.Remove(physicalCard);
    }

    public override void SummonCards(int space, int cardsInDeck)
    {
        if (space > 1 && cardsInDeck >= 2)
        {
            DrawNumCards(2);
            enemyManager.mana -= 2;
        }
        else if (space == 1 || cardsInDeck == 1)
        {
            DrawNumCards(1);
            enemyManager.mana -= 2;
        }
    }
}
