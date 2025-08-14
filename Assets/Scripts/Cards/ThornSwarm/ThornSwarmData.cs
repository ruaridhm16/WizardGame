using UnityEngine;

[CreateAssetMenu(fileName = "ThornSwarmData", menuName = "Cards/ThornSwarmData")]
public class ThornSwarmData : CardData
{
    public int baseDamage = 10;

    public override Card CreateInstance()
    {
        ThornSwarm card = new ThornSwarm
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
