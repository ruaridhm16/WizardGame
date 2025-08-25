using UnityEngine;

public class Lifegel : Card
{
    public int healAmount;

    public override void OnCast()
    {
        PlayerValueManager.Health += healAmount;
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
