using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{

    public Card card;

    public void SetCard(Card cardData)
    {
        card = cardData;

        GetComponent<SpriteRenderer>().sprite = cardData.cardFace;
    }

    public void OnClick()
    {
        card.OnActivation();
    }

}
