using UnityEngine;

[CreateAssetMenu(fileName = "ThornSwarmData", menuName = "Cards/ThornSwarmData")]
public class ThornSwarmData : CardData
{
    public int baseDamage = 10;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        ThornSwarm card = new ThornSwarm
        {
            battleManager = BattleManager,
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
