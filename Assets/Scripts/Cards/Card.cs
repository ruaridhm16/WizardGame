using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public abstract class Card
{
    public BattleManager battleManager;
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    public string cardDescription;
    public string castDescription;
    public string bindDescription;
    public int cardHealth;

    public enum CardAttribute
    {
        Damage,
        Support,
        Healing,
        Aimable,
        Debuff,
        Buff,
        ManaGeneration,
        Binding,
        Casting,
        Trap,
    }

    public List<CardAttribute> CardAttributes;

    [HideInInspector] public bool isFlipped;
    [HideInInspector] public GameObject spawnedCard = null;
    [HideInInspector] public GameObject attatchedBindSlot = null;
    [HideInInspector] public bool isBound = false;
    [HideInInspector] public bool isPlayerCard;


    public virtual void OnCast(BattleManager.CastTargets target)
    {
        return;
    }
    public virtual void OnDestroyCard()
    {
        if (attatchedBindSlot != null)
        {
            attatchedBindSlot.GetComponent<BindSlot>().boundCard = null;
            attatchedBindSlot.GetComponent<BindSlot>().occupied = false;
        }
        GameObject.Destroy(spawnedCard);
    }
    public virtual void OnDraw()
    {
        return;
    }
    public virtual void OnDiscard()
    {
        return;
    }
    public virtual void OnEnemyDestroyCard()
    {
        return;
    }
    public virtual void OnEnemyDamageCard()
    {
        return;
    }
    public virtual void OnBindPassive()
    {
        return;
    }
    public virtual void OnBind(bool player)
    {
        return;
    }

    public virtual void DamageCard(int damage)
    {
        cardHealth -= damage;
        if (cardHealth <= 0)
        {
            OnDestroyCard();
        }
    }
}
