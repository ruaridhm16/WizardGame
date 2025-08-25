using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Splines.SplineInstantiate;

public abstract class CardActions : MonoBehaviour
{
    public GameObject cardPrefab;
    public HandManager handManager;
    public GameObject handZone;

    public abstract IEnumerator DrawInitialHand();
    public abstract void DrawCard(Card card);
    public abstract void DrawNumCards(int numCards);

    public void DestroyRandomCards()
    {

    }

    public void DiscardSelectedCards()
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

    public void CastSelectedCards()
    {
        int cost = CalculateCastBindManaCost();

        foreach (Card card in DeckManager.SelectedCards)
        {
            GameObject physicalCard = card.spawnedCard;

            card.OnCast();

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

    public void BindSelectedCards()
    {
        int cost = CalculateCastBindManaCost();

        int numSelected = DeckManager.SelectedCards.Count;

        for (int i = 0; i < numSelected; i++)
        {
            Card card = DeckManager.SelectedCards[0];

            GameObject physicalCard = card.spawnedCard;
            GameObject targetParent = DeckManager.BoundSlots.Find(o => o.GetComponent<BindSlot>().occupied == false);

            Bind(card, targetParent);
            
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

    public void Bind(Card card, GameObject destination)
    {
        GameObject physicalCard = card.spawnedCard;

        physicalCard.transform.parent = destination.transform;
        physicalCard.transform.localPosition = Vector3.zero;
        physicalCard.transform.localScale = Vector3.one;
        physicalCard.GetComponent<CardView>().Flip();
        destination.GetComponent<BindSlot>().occupied = true;
        destination.GetComponent<BindSlot>().boundCard = card;

        physicalCard.GetComponent<CardInteractions>().Deselect();
        physicalCard.GetComponent<CardInteractions>().isInteractible = false;
        
        DeckManager.Hand.Remove(card);
        DeckManager.HandCards.Remove(physicalCard);
    }

    public int CalculateCastBindManaCost()
    {
        int total = 0;

        foreach (Card card in DeckManager.SelectedCards)
        {
            total += card.manaCost;
        }

        return total;
    }

    public void SummonCards(int space, int cardsInDeck)
    {
        if (space > 1 && cardsInDeck >= 2)
        {
            GetComponent<CardActions>().DrawNumCards(2);
            PlayerValueManager.Mana -= 2;
        }
        else if (space == 1 || cardsInDeck == 1)
        {
            GetComponent<CardActions>().DrawNumCards(1);
            PlayerValueManager.Mana -= 2;
        }

        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurnComplete = true;
    }


}
