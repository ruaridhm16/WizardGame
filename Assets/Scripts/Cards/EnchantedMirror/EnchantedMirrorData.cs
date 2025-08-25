using UnityEngine;

[CreateAssetMenu(fileName = "EnchantedMirrorData", menuName = "Cards/EnchantedMirrorData")]
public class EnchantedMirrorData : CardData
{
    public int damageReflectionPeercentge = 5;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        EnchantedMirror card = new EnchantedMirror
        {
            battleManager = BattleManager,
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            damageReflectionPeercentge = this.damageReflectionPeercentge
        };
        return card;
    }
}
