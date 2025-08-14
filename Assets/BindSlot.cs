using System;
using UnityEditor.UI;
using UnityEngine;

public class BindSlot : MonoBehaviour
{
    [HideInInspector] public bool occupied = false;
    [HideInInspector] public bool disabled = false;
    [HideInInspector] public Card boundCard;
    void Start()
    {
        DeckManager.BoundSlots.Add(this.gameObject);
    }


    private void OnTurnEnd()
    {
        if (occupied)
        {
            Debug.Log(this.name + " activated " + boundCard.name);
        }
    }
}
