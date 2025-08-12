using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Mana Berry")]
public class Mana_Berry : Card
{
    public int manaRegen;

    public override void OnActivation()
    {
        Debug.Log($"{cardName} generated {manaRegen} mana!");
    }
}
