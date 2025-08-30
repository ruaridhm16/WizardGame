using UnityEngine;

public class ThunderStrike : Card
{
    public int damage;

    public override void OnBind(bool player)
    {
        return;
    }

    public override void OnBindPassive()
    {
        return;
    }

    public override void OnCast(BattleManager.CastTargets target)
    {
        switch (target)
        {
            case BattleManager.CastTargets.Player:
                PlayerValueManager.DamagePlayer(damage);
                break;
            case BattleManager.CastTargets.Opponent:
                battleManager.enemyManager.DealDamage(damage);
                break;
            case BattleManager.CastTargets.PlayerBoundCard:
                break;
            case BattleManager.CastTargets.OpponentBoundCard:
                break;
        }
    }

    public override void OnDestroyCard()
    {
        return;
    }

    public override void OnDiscard()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDraw()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnemyDamageCard()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnemyDestroyCard()
    {
        throw new System.NotImplementedException();
    }
}
