using UnityEngine;

public class Lifegel : Card
{
    public int healAmount;

    public override void OnCast(BattleManager.CastTargets target)
    {
        switch (target)
        {
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
}
