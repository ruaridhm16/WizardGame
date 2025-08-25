using UnityEngine;

public class PlayerHandManager : HandManager
{
    public override void UpdateHandView()
    {
        BoxCollider2D handZoneCollider = GetComponent<BoxCollider2D>();

        ResizeBounds(DeckManager.Hand.Count);

        for (int i = 0; i < DeckManager.Hand.Count; i++)
        {
            if (DeckManager.HandCards[i].GetComponent<CardInteractions>().isDragging == false)
            {
                DeckManager.HandCards[i].transform.position = new Vector2(handZoneCollider.bounds.min.x + (handZoneCollider.bounds.size.x / DeckManager.Hand.Count) * (i + 0.5f), handZoneCollider.bounds.center.y);

                SpriteRenderer sr = DeckManager.HandCards[i].GetComponent<SpriteRenderer>();
                sr.sortingOrder = i;

            }

        }
    }
    
    public override void ResizeBounds(int numCards)
    {
        BoxCollider2D handZoneCollider = GetComponent<BoxCollider2D>();
        

        if (DeckManager.Hand.Count <= 10)
        {
            handZoneCollider.size = new Vector2(boxUnit * numCards + ((10-numCards) * 0.4f), handZoneCollider.size.y);
        }
    }
}
