using UnityEngine;

public abstract class Card
{
    public BattleManager battleManager;
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    public string decription;
    public int cardHealth;
    [HideInInspector] public bool isFlipped;
    [HideInInspector] public GameObject spawnedCard = null;
    [HideInInspector] public bool isBound = false;
    [HideInInspector] public bool isPlayerCard;


    public abstract void OnCast(BattleManager.CastTargets target);
    public abstract void OnDestroyCard();
    public abstract void OnDraw();
    public abstract void OnDiscard();
    public abstract void OnEnemyDestroyCard();
    public abstract void OnEnemyDamageCard();
    public abstract void OnBindPassive();
    public abstract void OnBind(bool player);

    public void DamageCard(int damage)
    {
        cardHealth -= damage;
        if (cardHealth <= 0)
        {
            OnDestroyCard();
        }
    }


}
