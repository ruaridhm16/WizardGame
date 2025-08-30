using UnityEngine;

public class DarkOrb : Card
{
    public int damage;

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
}
