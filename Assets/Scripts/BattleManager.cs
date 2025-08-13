using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private CardActions CardActions;
    [SerializeField] private GameObject HandZone;

    void Start()
    {
        DeckManager.HandZone = HandZone;

        CardLibrary.Initialize();

        StartBattle();
    }

    public void StartBattle()
    {
        //Add cards to the player's deck
        for (int i=0; i<numFireball; i++)
        {
            DeckManager.SetDeck.Add(FireballSO);
        }
        for (int i=0; i<numLifegel; i++)
        {
            DeckManager.SetDeck.Add(LifegelSO);
        }
        for (int i=0; i<numManaberry; i++)
        {
            DeckManager.SetDeck.Add(ManaBerrySO);
        }
        for (int i=0; i<numThunderStrike; i++)
        {
            DeckManager.SetDeck.Add(ThunderStrikeSO);
        }
        for (int i=0; i<numEnchantedMirror; i++)
        {
            DeckManager.SetDeck.Add(EnchantedMirrorSO);
        }
        for (int i=0; i<numDarkOrb; i++)
        {
            DeckManager.SetDeck.Add(DarkOrbSO);
        }
        DeckManager.Deck = new List<Card>(DeckManager.SetDeck);

        StartCoroutine(CardActions.DrawInitialHand(PlayerValueManager.handDrawSize));
    }

    public IEnumerator ActiveBattle()
    {
        yield return new WaitForEndOfFrame();
    }

    
}
