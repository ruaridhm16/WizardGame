using UnityEngine;

[CreateAssetMenu(fileName = "FireballData", menuName = "Cards/FireballData")]
public class FireballData : CardData
{

    public int baseDamage = 10;

    public override Card CreateInstance()
    {
        Fireball card = new Fireball
        {
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            damage = this.baseDamage
        };
        return card;
    }
}
