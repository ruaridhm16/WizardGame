using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
public class EnemyManager : MonoBehaviour
{
    public int MaxHealth = 50;
    public int health = 50;
    public int mana = 10;

    public int manaRegen = 2;

    [HideInInspector] public BattleManager battleManager;
    [HideInInspector] public EnemyCardActions enemyCardActions;

    public GameObject enemyHandZone;
    public List<GameObject> BoundSlots = new List<GameObject>();

    [SerializeField] public List<Card> enemyDeck = new List<Card>();
    [HideInInspector] public List<Card> enemyHand = new List<Card>();
    [HideInInspector] public List<GameObject> enemyHandCards = new List<GameObject>();
    [HideInInspector] public List<Card> enemySelectedCards = new List<Card>();
    [HideInInspector] public List<GameObject> enemySelectedPhysicalCards = new List<GameObject>();
    [HideInInspector] public List<Card> enemyDiscards = new List<Card>();

    void Start()
    {
        battleManager = GetComponent<BattleManager>();
        enemyCardActions = GetComponent<EnemyCardActions>();
    }

    public void DealDamage(int damage)
    {
        health -= damage;
    }

    public void EnemyAction()
    {
        if (doSkip())
        {
            SkipTurn();
            print("Enemy Skips with " + mana + " mana");
        }
        else if (doSummon())
        {
            SummonCards();
            print("Enemy Summons");
        }
        else if (doBind())
        {
            BindCards();
            print("Enemy Binds");
        }
        else
        {
            CastCards();
            print("Enemy Casts");
        }

    }
    public bool doSkip()
    {
        if (mana <= 0) { return true; }
        if (mana == 1 && PercentageChance(75)) { return true; }
        if (mana == 2 && PercentageChance(50)) { return true; }
        if ((mana <= 4) && PercentageChance(20)) { return true; }
        if ((mana <= 6) && PercentageChance(5)) { return true; }
        return false;
    }

    public bool doSummon()
    {
        if (enemyHand.Count == 0) { return true; }
        if (enemyHand.Count <= 2 && PercentageChance(75)) {return true;}
        if (enemyHand.Count <= 4 && PercentageChance(50)) {return true;}
        if (PercentageChance(25)) { return true;}
        return false;
    }

    public bool doBind()
    {
        int chance = 20;
        foreach (Card card in enemyHand)
        {
            if (card.CardAttributes.Contains(Card.CardAttribute.Binding))
            {
                chance += 10;
            }
        }

        return PercentageChance(chance);
    }

    public bool BindSlotsAllFilled()
    {
        bool allFilled = true;
        foreach (GameObject bindSlot in BoundSlots)
        {
            allFilled = allFilled && bindSlot.GetComponent<EnemyBindSlot>().occupied;
        }

        return allFilled;
    }

    public bool PercentageChance(int Percent)
    {
        return (UnityEngine.Random.Range(0, 100) <= Percent);
    }

    public void HealEnemy(int healAmount)
    {
        health += healAmount;
    }

    public void SummonCards()
    {
        enemyCardActions.SummonCards(PlayerValueManager.handDrawSize - enemyHand.Count, enemyDeck.Count);
        battleManager.enemyTurnComplete = true;
    }

    public void SkipTurn()
    {
        battleManager.enemyTurnComplete = true;
    }

    public int GetNumBindSpaces()
    {
        int numFilled = 0;
        foreach (GameObject bindSlot in BoundSlots)
        {
            if (!bindSlot.GetComponent<EnemyBindSlot>().occupied) {numFilled += 1;}
        }
        return numFilled;
    }

    public void BindCards()
    {
        int currentTotalManaCost = 0;
        int bindSpaces = GetNumBindSpaces();
        int baseChance = 50;

        for (int i = 0; i < enemyHand.Count; i++)
        {
            int chance = baseChance;
            if (enemyHand[i].CardAttributes.Contains(Card.CardAttribute.Binding)) { chance += 50; }
            chance -= 10 * i;
            if (mana - currentTotalManaCost > 0 && bindSpaces > 0 && PercentageChance(chance))
            {
                enemySelectedCards.Add(enemyHand[i]);
                enemySelectedPhysicalCards.Add(enemyHand[i].spawnedCard);
                currentTotalManaCost += enemyHand[i].manaCost;
            }
        }

        if (enemySelectedCards.Count == 0) {
            int a = UnityEngine.Random.Range(0, enemyHand.Count - 1);
            enemySelectedCards.Add(enemyHand[a]);
            enemySelectedPhysicalCards.Add(enemyHand[a].spawnedCard);
        }

        enemyCardActions.BindSelectedCards();
        battleManager.enemyTurnComplete = true;
    }

    public void CastCards()
    {
        int currentTotalManaCost = 0;

        for (int i = 0; i < enemyHand.Count; i++)
        {
            if (mana - currentTotalManaCost > 0 && enemyHand[i].CardAttributes.Contains(Card.CardAttribute.Casting) && PercentageChance(50))
            {
                enemySelectedCards.Add(enemyHand[i]);
                enemySelectedPhysicalCards.Add(enemyHand[i].spawnedCard);
                currentTotalManaCost += enemyHand[i].manaCost;
            }
        }

        if (enemySelectedCards.Count == 0)
        {
            int a = UnityEngine.Random.Range(0, enemyHand.Count - 1);
            enemySelectedCards.Add(enemyHand[a]);
            enemySelectedPhysicalCards.Add(enemyHand[a].spawnedCard);
        }

        enemyCardActions.CastSelectedCards(BattleManager.CastTargets.Player);
        battleManager.enemyTurnComplete = true;
    }

    public void SortBoundSlots()
    {
        BoundSlots.Sort((a, b) =>
        {
            if (a == null && b == null) return 0;
            if (a == null) return 1;
            if (b == null) return -1;

            return string.Compare(a.name, b.name, System.StringComparison.Ordinal);
        });
    }

    public void healEnemy(int healAmount)
    {
        health += healAmount;
    }

    public void giveMana(int manaAmount)
    {
        mana += manaAmount;
    }
}
