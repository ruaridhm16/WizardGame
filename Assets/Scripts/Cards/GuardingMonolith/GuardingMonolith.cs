using UnityEngine;

public class GuardingMonolith : Card
{
    public int protection;

    public override void OnBind(bool player)
    {
        if (player)
        {
            foreach (GameObject slot in DeckManager.BoundSlots)
            {
                Card card = slot.GetComponent<BindSlot>().boundCard;
                if (card != null && card != this)
                {
                    card.cardHealth += 5;
                }
            }
        }
        else
        {
            foreach (GameObject slot in battleManager.enemyManager.BoundSlots)
            {
                Card card = slot.GetComponent<EnemyBindSlot>().boundCard;
                if (card != null && card != this)
                {
                    card.cardHealth += 5;
                }
            }
        }
    }

    public override void OnDestroyCard()
    {
        base.OnDestroyCard();

        if (isPlayerCard)
        {
            foreach (GameObject slot in DeckManager.BoundSlots)
            {
                Card card = slot.GetComponent<BindSlot>().boundCard;
                if (card != null && card != this)
                {
                    card.cardHealth -= 5;
                }
            }
        }
        else
        {
            foreach (GameObject slot in battleManager.enemyManager.BoundSlots)
            {
                Card card = slot.GetComponent<EnemyBindSlot>().boundCard;
                if (card != null && card != this)
                {
                    card.cardHealth -= 5;
                }
            }
        }
    }

}
