using UnityEngine;

[CreateAssetMenu(fileName = "DarkOrbData", menuName = "Cards/DarkOrbData")]
public class DarkOrbData : CardData
{
    public int baseDamage = 10;

    public override Card CreateInstance()
    {
        DarkOrb card = new DarkOrb
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
