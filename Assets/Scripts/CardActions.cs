using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Splines.SplineInstantiate;

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
        card.spawnedCard = physicalCard;
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

        // Play card casting animation 


        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurn = false;
    }

    public void BindSelectedCards()
    {
        int total = 0;

        foreach (Card card in DeckManager.SelectedCards)
        {
            total += card.manaCost;
        }

        int numSelected = DeckManager.SelectedCards.Count;

        for (int i = 0; i < numSelected; i++)
        {
            GameObject physicalCard = DeckManager.SelectedPhysicalCards[0];
            
            Card card = physicalCard.GetComponent<CardView>().card;

            DeckManager.Hand.Remove(card);
            DeckManager.HandCards.Remove(physicalCard);

            GameObject targetParent = DeckManager.BoundSlots.Find(o => o.GetComponent<BindSlot>().occupied == false);
            physicalCard.transform.parent = targetParent.transform;
            physicalCard.transform.localPosition = Vector3.zero;
            physicalCard.transform.localScale = Vector3.one;
            physicalCard.GetComponent<CardView>().Flip();
            targetParent.GetComponent<BindSlot>().occupied = true;
            targetParent.GetComponent<BindSlot>().boundCard = card;

            physicalCard.GetComponent<CardInteractions>().Deselect();
            physicalCard.GetComponent<CardInteractions>().isInteractible=false;
            DeckManager.SelectedPhysicalCards.Remove(physicalCard);
            DeckManager.SelectedCards.Remove(card);


        }
        DeckManager.SelectedCards.Clear();
        DeckManager.SelectedPhysicalCards.Clear();
        DeckManager.HandZone.GetComponent<HandManager>().UpdateHandView();

        PlayerValueManager.Mana -= total;

        // Play card binding animation 

        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurn = false;
    }

    public void SummonCards( int space, int cardsInDeck)
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

        GameObject.Find("BattleManager").GetComponent<BattleManager>().playerTurn = false;
    }


}
