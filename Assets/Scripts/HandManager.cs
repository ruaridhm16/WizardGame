using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateHandView() {
        BoxCollider2D handZoneCollider = GetComponent<BoxCollider2D>();
        
        for (int i = 0; i < DeckManager.Hand.Count; i++)
        {
            if (DeckManager.HandCards[i].GetComponent<CardInteractions>().isDragging == false) {
            DeckManager.HandCards[i].transform.position = new Vector2(handZoneCollider.bounds.min.x + (handZoneCollider.bounds.size.x / DeckManager.Hand.Count) * (i + 0.5f), handZoneCollider.bounds.center.y);
            
            SpriteRenderer sr = DeckManager.HandCards[i].GetComponent<SpriteRenderer>();
            sr.sortingOrder = i;
            
            }
            
        }
    }

    public IEnumerator SpawnCards(GameObject cardPrefab, int numCards)
    {
        BoxCollider2D handZoneCollider = GetComponent<BoxCollider2D>();

        for (int i = 0; i < numCards; i++)
        {
            Card spawnedCard = DeckManager.Deck[0];


            GameObject physicalCard = Instantiate(cardPrefab, position: new Vector2(handZoneCollider.bounds.min.x + (handZoneCollider.bounds.size.x / numCards) * (i + 0.5f), handZoneCollider.bounds.center.y), rotation: quaternion.identity, parent: DeckManager.HandZone.transform);
            SpriteRenderer sr = physicalCard.GetComponent<SpriteRenderer>();
            sr.sortingOrder = i;
            physicalCard.GetComponent<CardView>().SetCard(spawnedCard);

            DeckManager.Hand.Add(spawnedCard);
            DeckManager.HandCards.Add(physicalCard);
            spawnedCard.SpawnedPhysicalCard = physicalCard;
            DeckManager.Deck.Remove(spawnedCard);

            yield return new WaitForSeconds(0.05f);
        }
    }
}
