using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrikeData", menuName = "Cards/ThunderStrikeData")]
public class ThunderStrikeData : CardData
{
    public int baseDamage = 10;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        ThunderStrike card = new ThunderStrike
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
