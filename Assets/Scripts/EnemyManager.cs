using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public int MaxHealth = 100;
    public int health = 100;
    public int mana = 10;

    public GameObject enemyHandZone;
    public List<GameObject> BoundSlots = new List<GameObject>();

    [SerializeField] public List<Card> enemyDeck = new List<Card>();
    [HideInInspector] public List<Card> enemyHand = new List<Card>();
    [HideInInspector] public List<GameObject> enemyHandCards = new List<GameObject>();
    [HideInInspector] public List<Card> enemySelectedCards = new List<Card>();
    [HideInInspector] public List<Card> enemySelectedPhysicalCards = new List<Card>();
    [HideInInspector] public List<Card> enemyDiscards = new List<Card>();


    public void DealDamage(int damage)
    {
        health -= damage;
    }

    public void EnemyAction()
    {

    }

    public void HealEnemy(int healAmount)
    {

    }

    public void DrawEnemyCards()
    {

    }

    public void MakeEnemyDecision()
    {

    }

    public void SelectEnemyCards()
    {

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
}
