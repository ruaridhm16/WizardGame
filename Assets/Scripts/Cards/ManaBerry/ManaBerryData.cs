using UnityEngine;

[CreateAssetMenu(fileName = "ManaBerryData", menuName = "Cards/ManaBerryData")]
public class ManaBerryData : CardData
{
    public int baseManaRestore = 2;

    public override Card CreateInstance()
    {
        ManaBerry card = new ManaBerry
        {
            cardName = this.cardName,
            manaCost = this.manaCost,
            cardFace = this.cardFace,
            cardBack = this.cardBack,
            isFlipped = false,
            manaRestore = this.baseManaRestore
        };
        return card;
    }
}
