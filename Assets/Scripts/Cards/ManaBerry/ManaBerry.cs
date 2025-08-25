using UnityEngine;

public class ManaBerry : Card
{
    public int manaRestore;
    public int instantManaGain;

    public override void OnCast()
    {
        PlayerValueManager.gainMana(instantManaGain);
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
