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

    public IEnumerator ActiveBattle()
    {
        yield return new WaitForEndOfFrame();
    }

    
}
