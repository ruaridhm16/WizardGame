using UnityEngine;

[CreateAssetMenu(fileName = "ManaBerryData", menuName = "Cards/ManaBerryData")]
public class ManaBerryData : CardData
{
    public int baseManaRestore = 2;
    public int baseInstantManaGain = 5;

    public override Card CreateInstance(BattleManager BattleManager)
    {
        ManaBerry card = new ManaBerry
        {
            battleManager = BattleManager,
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            manaRestore = this.baseManaRestore,
            instantManaGain = this.baseInstantManaGain
        };
        return card;
    }
}
