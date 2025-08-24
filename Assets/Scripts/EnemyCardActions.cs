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
            DrawCard(enemyManager.enemyDeck[UnityEngine.Random.Range(0, enemyManager.enemyHand.Count - 1)]);

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
        sr.sprite = card.cardBack;
        sr.enabled = true;
        physicalCard.transform.localScale = Vector2.one;
        card.isFlipped = true;
        physicalCard.GetComponent<CardInteractions>().isEnemyCard = true;
        handManager.UpdateHandView();
    }
    public override void DrawNumCards(int numCards)
    {
        for (int i = 0; i < numCards; i++) {
            DrawCard(enemyManager.enemyDeck[UnityEngine.Random.Range(0, enemyManager.enemyDeck.Count - 1)]);
        }
    }
}
