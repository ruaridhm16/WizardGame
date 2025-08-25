using UnityEngine;

public class ThunderStrike : Card
{
    public int damage;

    public override void OnCast()
    {
        battleManager.enemyManager.DealDamage(damage);
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
