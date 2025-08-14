using UnityEngine;

public class CardView : MonoBehaviour
{
    public Card card;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCard(Card card)
    {
        this.card = card;
        spriteRenderer.sprite = card.cardFace;
    }

    public void Flip()
    {
        if (card.flipped)
        {
            spriteRenderer.sprite = card.cardFace;
        }
        else
        {
            spriteRenderer.sprite = card.cardBack;
        }

        card.flipped = !card.flipped;
    }
}
