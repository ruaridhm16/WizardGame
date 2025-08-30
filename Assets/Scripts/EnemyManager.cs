using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
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
        /// Possible actions are:
        /// Pass (2 mana or less)
        /// Summon (<= 1 card 100%, 2 cards 50%, 3 cards 25%, 4 cards 10%)
        /// Bind (20% chance to bind)
        /// Cast else
        /// 

        if (mana <= 2)
        {
            SkipTurn();
            print("ENEMY SKIPS");
        }
        else if (enemyHand.Count <= 1 || (enemyHand.Count == 2 && PercentageChance(50)) || (enemyHand.Count == 3 && PercentageChance(25)) || (enemyHand.Count == 4 && PercentageChance(10)))
        {
            SummonCards();
            print("ENEMY SUMMONS");
        }
        else if (!BindSlotsAllFilled() && PercentageChance(25))
        {
            BindCards();
            print("ENEMY BINDS");
        }
        else
        {
            CastCards();
            print("ENEMY CASTS");
        }

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
        return (Random.Range(0, 100) <= Percent);
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
        bool allSelected = false;
        int currentTotalManaCost = 0;
        int bindSpaces = GetNumBindSpaces();
        while (!allSelected)
        {
            Card addedCard = enemyHand[Random.Range(0, enemyHand.Count - 1)];
            if (addedCard.manaCost <= mana)
            {
                enemySelectedCards.Add(addedCard);
                enemySelectedPhysicalCards.Add(addedCard.spawnedCard);
                currentTotalManaCost += addedCard.manaCost;
                if ((mana - currentTotalManaCost <= 2 || PercentageChance(50)) || (enemySelectedCards.Count + 1 >= bindSpaces))
                {
                    allSelected = true;
                }
            }
        }
        enemyCardActions.BindSelectedCards();
        battleManager.enemyTurnComplete = true;
    }

    public void CastCards()
    {
        bool allSelected = false;
        int currentTotalManaCost = 0;
        while (!allSelected)
        {
            Card addedCard = enemyHand[Random.Range(0, enemyHand.Count - 1)];
            if (addedCard.manaCost <= mana)
            {
                enemySelectedCards.Add(addedCard);
                enemySelectedPhysicalCards.Add(addedCard.spawnedCard);
                currentTotalManaCost += addedCard.manaCost;
                if (mana - currentTotalManaCost <= 2 || PercentageChance(50)) {
                    allSelected = true;
                }
            }
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
