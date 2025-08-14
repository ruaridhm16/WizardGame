using UnityEngine;

public class EnchantedMirror : Card
{
    public int damageReflectionPeercentge;

    public override void OnCast()
    {
        Debug.Log($"{cardName} deals {damageReflectionPeercentge} damage!");
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
