using UnityEngine;

[CreateAssetMenu(fileName = "LifegelData", menuName = "Cards/LifegelData")]
public class LifegelData : CardData
{
    public int baseHealAmount = 10;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        Lifegel card = new Lifegel
        {
            battleManager = BattleManager,
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            healAmount = this.baseHealAmount
        };
        return card;
    }
}
