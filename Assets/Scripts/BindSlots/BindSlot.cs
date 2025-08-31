using System;
using UnityEditor.UI;
using UnityEngine;

public class BindSlot : MonoBehaviour
{
    public bool occupied = false;
    public bool disabled = false;
    public Card boundCard = null;
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

    void Update()
    {
        if(transform.childCount > 0) { occupied = true; } else { occupied = false; }
    }

}
