using UnityEngine;

public class Lifegel : Card
{
    public int healAmount;

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
            //Target = Player ->  gives the player health
            case BattleManager.CastTargets.Player:
                battleManager.enemyManager.healEnemy(healAmount);
                break;
            case BattleManager.CastTargets.Opponent:
                PlayerValueManager.healPlayer(healAmount);
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
