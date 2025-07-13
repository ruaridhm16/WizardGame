using UnityEngine;

public class Fireball : Card
{
    public int damage = 4;

    void Start()
    {
        cardName = "Fireball";
        manaCost = 2;

    }


    public override void CardEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void CardPassiveEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCardDeletion()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCardDrag()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCardDrop()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCardEntry()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCardSelect()
    {
        throw new System.NotImplementedException();
    }
}
