using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    bool battleStarted = false;
    [SerializeField] public List<Card> initialDeck;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !battleStarted)
        {
            battleStarted = true;
            SetupBattle();
        }
    }

    void SetupBattle()
    {
        foreach (Card card in initialDeck)
        {
            GameObject cardObject = Instantiate(card.gameObject);
            Card instantiatedCard = cardObject.GetComponent<Card>();
            DeckManager.cardDeck.Add(instantiatedCard);
            print(instantiatedCard.cardName + " added to deck");
        }
    }
}
