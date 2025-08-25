using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
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

    [SerializeField] private PlayerCardActions PlayerCardActions;
    [SerializeField] private EnemyCardActions EnemyCardActions;
    [SerializeField] private GameObject PlayerHandZone;
    [SerializeField] private GameObject EnemyHandZone;

    [SerializeField] public EnemyManager enemyManager;

    public bool playerTurnComplete = false;

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

        AddEnemyNumCards(15, FireballSO);
        AddEnemyNumCards(5, LifegelSO);

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
    
    public void AddEnemyNumCards(int num, CardData card)
    {
        for (int i = 0; i < num; i++)
        {
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
        print("Entered Player turn");
        phase = BattlePhase.PlayerTurn;
        StartCoroutine(AwaitBoolean(() => playerTurnComplete, () => nextPhase()));
    }

    public void EnterPlayerAnimation()
    {
        playerTurnComplete = false;
        print("Entered Player Animations");
        phase = BattlePhase.PlayerTurnAnimations;

        //Animate

        StartCoroutine(AwaitBoolean(() => true, () => nextPhase()));
    }
    public void EnterPlayerPostTurn()
    {
        print("Entered Player Post turn");
        phase = BattlePhase.PlayerPostTurn;

        //Some Passives will active here

        StartCoroutine(AwaitBoolean(() => true, () => nextPhase()));
    }

    public void EnterOpponentTurn()
    {
        print("Entered Opponent turn");
        phase = BattlePhase.OpponentTurn;

        //Opponent move

        StartCoroutine(AwaitBoolean(() => true, () => nextPhase()));
    }

    public void EnterOpponentAnimations()
    {
        print("Entered Opponent Animations");
        phase = BattlePhase.OpponentTurnAnimations;

        //Animate

        StartCoroutine(AwaitBoolean(() => true, () => nextPhase()));
    }
    public void EnterOpponentPostTurn()
    {
        print("Entered Opponent post turn");
        phase = BattlePhase.OpponentPostTurn;

        StartCoroutine(AwaitBoolean(() => true, () => nextPhase()));
    }
    public void EnterPostRound()
    {
        print("Entered Post Round");
        phase = BattlePhase.PostRound;
        StartCoroutine(GetComponent<UIManager>().ManaRegenAnimation());
        StartCoroutine(AwaitBoolean(() => true, () => nextPhase()));
    }

    
}

