using UnityEngine;

[CreateAssetMenu(fileName = "DarkOrbData", menuName = "Cards/DarkOrbData")]
public class DarkOrbData : CardData
{
    public int baseDamage = 10;
    

    public override Card CreateInstance(BattleManager BattleManager)
    {
        DarkOrb card = new DarkOrb
        {
            battleManager = BattleManager,
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            damage = this.baseDamage,
            isPlayerCard = true,
            cardHealth = baseCardHealth,
        };
        return card;
    }
}
