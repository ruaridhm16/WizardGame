using UnityEngine;

[CreateAssetMenu(fileName = "GuardingMonolith", menuName = "Cards/GuardingMonolithData")]
public class GuardingMonolithData : CardData
{
    public int baseProtection = 10;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        GuardingMonolith card = new GuardingMonolith
        {
            battleManager = BattleManager,
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            protection = this.baseProtection,
            isPlayerCard = true,
            cardHealth = baseCardHealth,
            CardAttributes = BaseCardAttributes,
        };
        return card;
    }
}
