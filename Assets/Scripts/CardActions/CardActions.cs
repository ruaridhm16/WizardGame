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

    public abstract void CastSelectedCards(BattleManager.CastTargets castTarget);

    public abstract void DiscardSelectedCards();
    public abstract void BindSelectedCards();
    public abstract void Bind(Card card, GameObject destination);

    public abstract void SummonCards(int space, int cardsInDeck);

    public int CalculateCastBindManaCost(List<Card> selectedCards)
    {
        int total = 0;

        foreach (Card card in selectedCards)
        {
            total += card.manaCost;
        }

        return total;
    }

    


}
