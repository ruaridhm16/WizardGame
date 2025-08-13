using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

public class CastButtonHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnMouseDown()
    {
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
    }

    void PrintCardList(List<Card> list)
    {
        string str = "";
        foreach (Card card in list)
        {
            str += card.cardName + " ";
        }
        print(str);
    }

    // Update is called once per frame
    void Update()
    {
        if (DeckManager.SelectedCards.Count > 0) {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
