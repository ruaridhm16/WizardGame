using UnityEngine;

[CreateAssetMenu(fileName = "FireballData", menuName = "Cards/FireballData")]
public class FireballData : CardData
{

    public int baseDamage = 10;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        Fireball card = new Fireball
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
            CardAttributes = BaseCardAttributes,
        };
        return card;
    }
}
