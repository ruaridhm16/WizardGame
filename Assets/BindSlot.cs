using System;
using UnityEditor.UI;
using UnityEngine;

public class BindSlot : MonoBehaviour
{
    public bool occupied = false;
    public bool disabled = false;
    public GameObject boundCard;
    public BattleManager BattleManager;
    public bool lastFramePlayerTurn;
    void Start()
    {
        BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        lastFramePlayerTurn = BattleManager.playerTurn;
    }

    // Update is called once per frame
    void Update()
    {
        boundCard = (transform.childCount > 0) ? transform.GetChild(0).gameObject : null;
        if (boundCard != null) { occupied = true; }
    
        if (lastFramePlayerTurn == true && BattleManager.playerTurn == false && !disabled)
        {
            OnTurnEnd();
        }

        lastFramePlayerTurn = BattleManager.playerTurn;

    }

    private void OnTurnEnd()
    {
        if (occupied)
        {
            Debug.Log(this.name + " activated " + boundCard.name);
        }
    }
}
