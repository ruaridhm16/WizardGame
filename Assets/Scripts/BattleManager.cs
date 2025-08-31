using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //STARTING PLAYER DECK
    public int numFireball;
    public FireballData FireballSO;
    public int numLifegel;
    public LifegelData LifegelSO;
    public int numManaberry;
    public ManaBerryData ManaBerrySO;
    public int numThunderStrike;
    public ThunderStrikeData ThunderStrikeSO;
    public int numEnchantedMirror;
    public EnchantedMirrorData EnchantedMirrorSO;
    public int numDarkOrb;
    public DarkOrbData DarkOrbSO;
    public int numThornSwarm;
    public ThornSwarmData ThrornSwarmSO;
    public int numGuardingMonolith;
    public GuardingMonolithData guardingMonolithSO;



    //COMMON REFERENCES
    [SerializeField] private PlayerCardActions PlayerCardActions;
    [SerializeField] private EnemyCardActions EnemyCardActions;
    [SerializeField] private GameObject PlayerHandZone;
    [SerializeField] private GameObject EnemyHandZone;
    [SerializeField] public EnemyManager enemyManager;

    public bool playerTurnComplete = false;
    public bool enemyTurnComplete = false;

    //LETTING THE CARD SCRIPTS KNOW WHAT CARD IS THE TARGET
    public Card TargetedCard = null;

    

    public enum BattlePhase
    {
        PlayerTurn,
        PlayerTurnAnimations,
        PlayerPostTurn,

        OpponentTurn,
        OpponentTurnAnimations,
        OpponentPostTurn,

        PostRound
    }

    public BattlePhase phase = BattlePhase.PlayerTurn;

    public enum CastTargets
    {
        Player,
        Opponent,
        PlayerBoundCard,
        OpponentBoundCard,
        None
    }

    public CastTargets playerCastTarget = CastTargets.None;
    public CastTargets enemyCastTarget = CastTargets.None;

    public List<Card> lastCast = new List<Card>();
    
    [Header("Debugging")] [SerializeField] public List<string> statementSelectedCards;
    [SerializeField] public List<string> statementLastCast;

    void Start()
    {
        DeckManager.HandZone = PlayerHandZone;

        StartBattle();
    }


    public void StartBattle()
    {
        EnterPlayerTurn();

        AddNumCards(numFireball, FireballSO);
        AddNumCards(numLifegel, LifegelSO);
        AddNumCards(numManaberry, ManaBerrySO);
        AddNumCards(numDarkOrb, DarkOrbSO);
        AddNumCards(numThunderStrike, ThunderStrikeSO);
        AddNumCards(numEnchantedMirror, EnchantedMirrorSO);
        AddNumCards(numThornSwarm, ThrornSwarmSO);
        AddNumCards(numGuardingMonolith, guardingMonolithSO);

        AddEnemyNumCards(UnityEngine.Random.Range(10, 20), FireballSO);
        AddEnemyNumCards(UnityEngine.Random.Range(6, 15), LifegelSO);
        AddEnemyNumCards(UnityEngine.Random.Range(2, 5), ManaBerrySO);
        AddEnemyNumCards(UnityEngine.Random.Range(0, 6), DarkOrbSO);
        AddEnemyNumCards(UnityEngine.Random.Range(0, 4), ThunderStrikeSO);
        AddEnemyNumCards(UnityEngine.Random.Range(5, 8), EnchantedMirrorSO);
        AddEnemyNumCards(UnityEngine.Random.Range(0, 9), ThrornSwarmSO);
        AddEnemyNumCards(UnityEngine.Random.Range(0, 4), guardingMonolithSO);

        DeckManager.Deck = new List<Card>(DeckManager.SetDeck);

        StartCoroutine(PlayerCardActions.DrawInitialHand());
        EnemyCardActions.enemyManager = enemyManager;
        StartCoroutine(EnemyCardActions.DrawInitialHand());
        
        
        
    }

    public IEnumerator AwaitBoolean(Func<bool> condition, Action action)
    {
        yield return new WaitUntil(condition);
        action?.Invoke();
    }

    public void AddNumCards(int num, CardData card)
    {
        for (int i = 0; i < num; i++)
        {
            DeckManager.SetDeck.Add(card.CreateInstance(this.GetComponent<BattleManager>()));
        }
    }

    public IEnumerator PhaseTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        nextPhase();
    }
    
    public void AddEnemyNumCards(int num, CardData card)
    {
        for (int i = 0; i < num; i++)
        {
            if (card == null) { Debug.Log("card null, " + card); return; }
            enemyManager.enemyDeck.Add(card.CreateInstance(this.GetComponent<BattleManager>()));
        }
    }

    public void nextPhase()
    {
        if (phase == BattlePhase.PostRound) { UpdatePhase(BattlePhase.PlayerTurn); return; }
        BattlePhase[] phases = new BattlePhase[] { BattlePhase.PlayerTurn, BattlePhase.PlayerTurnAnimations, BattlePhase.PlayerPostTurn, BattlePhase.OpponentTurn, BattlePhase.OpponentTurnAnimations, BattlePhase.OpponentPostTurn, BattlePhase.PostRound };
        for (int i = 0; i < phases.Length; i++)
        {
            if (phase == phases[i])
            {
                UpdatePhase(phases[i + 1]);
                return;
            }
        }

    }

    public void UpdatePhase(BattlePhase phase) {
        switch (phase)
        {
            case BattlePhase.PlayerTurn:
                EnterPlayerTurn();
                break;
            case BattlePhase.PlayerTurnAnimations:
                EnterPlayerAnimation();
                break;
            case BattlePhase.PlayerPostTurn:
                EnterPlayerPostTurn();
                break;
            case BattlePhase.OpponentTurn:
                EnterOpponentTurn();
                break;
            case BattlePhase.OpponentTurnAnimations:
                EnterOpponentAnimations();
                break;
            case BattlePhase.OpponentPostTurn:
                EnterOpponentPostTurn();
                break;
            case BattlePhase.PostRound:
                EnterPostRound();
                break;
        }
    }

    public void EnterPlayerTurn()
    {
        phase = BattlePhase.PlayerTurn;
        StartCoroutine(AwaitBoolean(() => playerTurnComplete, () => StartCoroutine(PhaseTimeout(0))));
    }

    public void EnterPlayerAnimation()
    {
        GetComponent<UIManager>().targetingMode = false;
        playerTurnComplete = false;
        phase = BattlePhase.PlayerTurnAnimations;

        //Animate

        StartCoroutine(AwaitBoolean(() => true, () => StartCoroutine(PhaseTimeout(1))));
    }
    public void EnterPlayerPostTurn()
    {
        phase = BattlePhase.PlayerPostTurn;

        //Some Passives will active here

        StartCoroutine(AwaitBoolean(() => true, () => StartCoroutine(PhaseTimeout(1))));
    }

    public void EnterOpponentTurn()
    {
        phase = BattlePhase.OpponentTurn;

        enemyManager.EnemyAction();

        StartCoroutine(AwaitBoolean(() => enemyTurnComplete, () => StartCoroutine(PhaseTimeout(1))));
    }

    public void EnterOpponentAnimations()
    {
        phase = BattlePhase.OpponentTurnAnimations;

        //Animate

        StartCoroutine(AwaitBoolean(() => true, () => StartCoroutine(PhaseTimeout(1))));
    }
    public void EnterOpponentPostTurn()
    {
        phase = BattlePhase.OpponentPostTurn;

        StartCoroutine(AwaitBoolean(() => true, () => StartCoroutine(PhaseTimeout(1))));
    }
    public void EnterPostRound()
    {
        phase = BattlePhase.PostRound;
        foreach (GameObject slot in DeckManager.BoundSlots)
        {
            Card boundCard = slot.GetComponent<BindSlot>().boundCard;
            if (boundCard != null)
            {
                boundCard.OnBindPassive();
            }
        }
        enemyManager.mana += enemyManager.manaRegen;
        StartCoroutine(GetComponent<UIManager>().ManaRegenAnimation());
        StartCoroutine(AwaitBoolean(() => true, () => StartCoroutine(PhaseTimeout(1))));
    }

    void DebuggingSelectedandLastCast()
    {
        statementSelectedCards.Clear();

        foreach (Card selectedcard in DeckManager.SelectedCards)
        {
            statementSelectedCards.Add(selectedcard.cardName);
        }
        
        statementLastCast.Clear();

        foreach (Card lastcard in lastCast)
        {
            statementLastCast.Add(lastcard.cardName);
        }
    
    } 
}

