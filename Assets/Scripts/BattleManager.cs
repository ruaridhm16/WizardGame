using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int numFireball;
    public Fireball FireballSO;
    public int numLifegel;
    public Lifegel LifegelSO;
    public int numManaberry;
    public ManaBerry ManaBerrySO;
    public int numThunderStrike;
    public ThunderStrike ThunderStrikeSO;
    public int numEnchantedMirror;
    public EnchantedMirror EnchantedMirrorSO;
    public int numDarkOrb;
    public DarkOrb DarkOrbSO;
    public int numThornSwarm;
    public ThornSwarm ThrornSwarmSO;

    [SerializeField] private CardActions CardActions;
    [SerializeField] private GameObject HandZone;

    public bool playerTurn;
    public bool previousPlayerTurn;

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
        DeckManager.HandZone = HandZone;

        CardLibrary.Initialize();

        StartBattle();
    }

    private void Update()
    {
        if (previousPlayerTurn == false && playerTurn == true) { NewTurn(); };
        previousPlayerTurn = playerTurn;
        
    }

    public void NewTurn()
    {
        GetComponent<UIManager>().turnActive = true;
    }


    public void StartBattle()
    {
        
        AddNumCards(numFireball, FireballSO);
        AddNumCards(numLifegel, LifegelSO);
        AddNumCards(numManaberry, ManaBerrySO);
        AddNumCards(numDarkOrb, DarkOrbSO);
        AddNumCards(numThunderStrike, ThunderStrikeSO);
        AddNumCards(numEnchantedMirror, EnchantedMirrorSO);
        AddNumCards(numThornSwarm, ThrornSwarmSO);

        DeckManager.Deck = new List<Card>(DeckManager.SetDeck);

        StartCoroutine(CardActions.DrawInitialHand(PlayerValueManager.handDrawSize));
    }

    public void AddNumCards(int num, Card card) {
        for (int i = 0; i < num; i++)
        {
            DeckManager.SetDeck.Add(card);
        }
    }

    public void nextPhase()
    {
        switch (phase)
        {
            case BattlePhase.PlayerTurn:
                phase = BattlePhase.PlayerTurnAnimations;
                break;
            case BattlePhase.PlayerTurnAnimations:
                phase = BattlePhase.PlayerPostTurn;
                break;
            case BattlePhase.PlayerPostTurn:
                phase = BattlePhase.OpponentTurn;
                break;
            case BattlePhase.OpponentTurn:
                phase = BattlePhase.OpponentTurnAnimations;
                break;
            case BattlePhase.OpponentTurnAnimations:
                phase = BattlePhase.OpponentPostTurn;
                break;
            case BattlePhase.OpponentPostTurn:
                phase = BattlePhase.PostRound;
                break;
            case BattlePhase.PostRound:
                phase = BattlePhase.PlayerTurn;
                break;

        }
    }

    public IEnumerator ActiveBattle()
    {
        switch (phase) {
            case BattlePhase.PlayerTurn:
                //wait for player move
                break;
            case BattlePhase.PlayerTurnAnimations:
                //wait for player move
                break;
            case BattlePhase.PlayerPostTurn:
                //wait for player move
                break;
            case BattlePhase.OpponentTurn:
                //wait for player move
                break;
            case BattlePhase.OpponentTurnAnimations:
                //wait for player move
                break;
            case BattlePhase.OpponentPostTurn:
                //wait for player move
                break;
            case BattlePhase.PostRound:
                //post round
                break;

        }

        yield return new WaitForEndOfFrame();
    }

    
}

