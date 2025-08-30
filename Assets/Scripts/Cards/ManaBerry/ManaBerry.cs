using System.Runtime.InteropServices;
using UnityEngine;

public class ManaBerry : Card
{
    public int manaRestore;
    public int instantManaGain;

    public override void OnBind(bool player)
    {
        if (player)
        {
            PlayerValueManager.ManaRegen += manaRestore;
        }
        else
        {
            battleManager.enemyManager.manaRegen += manaRestore;
        }
    }

    public override void OnBindPassive()
    {
        
    }

    public override void OnCast(BattleManager.CastTargets target)
    {
        //Target = Player ->  gives the player mana
        switch (target)
        {
            case BattleManager.CastTargets.Player:
                battleManager.enemyManager.giveMana(instantManaGain);
                break;
            case BattleManager.CastTargets.Opponent:
                PlayerValueManager.gainMana(instantManaGain);
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
