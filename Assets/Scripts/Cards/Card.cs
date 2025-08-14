using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public int manaCost;
    public Sprite cardFace;
    public Sprite cardBack;
    public string decription;
    [HideInInspector] public bool isFlipped;
    [HideInInspector] public GameObject spawnedCard = null;


    public abstract void OnCast();
    public abstract void OnDestroyCard();
    public abstract void OnDraw();
    public abstract void OnDiscard();

}
