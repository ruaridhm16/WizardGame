using UnityEngine;

public class Lifegel : Card
{
    public int healAmount;

    public override void OnCast()
    {
        Debug.Log($"{cardName} deals {healAmount} damage!");
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
