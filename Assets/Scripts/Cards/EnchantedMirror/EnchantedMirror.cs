using UnityEngine;

public class EnchantedMirror : Card
{
    public int damageReflectionPeercentge;

    public override void OnDestroyCard()
    {
        if (isPlayerCard)
        {
            DeckManager.SelectedCards = battleManager.GetComponent<BattleManager>().lastCast;
            battleManager.GetComponent<PlayerCardActions>().CastSelectedCards(BattleManager.CastTargets.Opponent);
            DeckManager.SelectedCards.Clear();
        }
        else
        {
            battleManager.GetComponent<EnemyManager>().enemySelectedCards = battleManager.GetComponent<BattleManager>().lastCast;
            battleManager.GetComponent<EnemyCardActions>().CastSelectedCards(BattleManager.CastTargets.Player);
            battleManager.GetComponent<EnemyManager>().enemySelectedCards.Clear();
        }
        
        base.OnDestroyCard();
    }

}
