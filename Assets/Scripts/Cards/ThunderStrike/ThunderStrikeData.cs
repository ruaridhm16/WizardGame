using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrikeData", menuName = "Cards/ThunderStrikeData")]
public class ThunderStrikeData : CardData
{
    public int baseDamage = 10;

    public override Card CreateInstance()
    {
        ThunderStrike card = new ThunderStrike
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
