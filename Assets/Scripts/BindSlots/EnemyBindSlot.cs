using UnityEngine;

public class EnemyBindSlot : MonoBehaviour
{
    public EnemyManager enemyManager;
    [HideInInspector] public bool occupied = false;
    [HideInInspector] public bool disabled = false;
    [HideInInspector] public Card boundCard;
    void Start()
    {
        enemyManager.BoundSlots.Add(this.gameObject);
        enemyManager.SortBoundSlots();
    }


    private void OnTurnEnd()
    {
        if (occupied)
        {
            Debug.Log(this.name + " activated " + boundCard.cardName);
        }
    }
}
