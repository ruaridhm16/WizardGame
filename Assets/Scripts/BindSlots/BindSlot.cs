using System;
using UnityEditor.UI;
using UnityEngine;

public class BindSlot : MonoBehaviour
{
    public BattleManager battleManager;
    [HideInInspector] public bool occupied = false;
    [HideInInspector] public bool disabled = false;
    [HideInInspector] public Card boundCard = null;
    void Start()
    {
        DeckManager.BoundSlots.Add(this.gameObject);
        DeckManager.SortBoundSlots();
    }


    public void ActivateCardPassive()
    {
        if (occupied)
        {
            boundCard.OnBindPassive();
        }
    }
}
