using UnityEngine;

public class ManaBerry : Card
{
    public int manaRestore;

    public override void OnCast()
    {
        Debug.Log($"{cardName} restores {manaRestore} mana!");
    }

    public override void OnDestroyCard()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDiscard()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDraw()
    {
        throw new System.NotImplementedException();
    }
}
