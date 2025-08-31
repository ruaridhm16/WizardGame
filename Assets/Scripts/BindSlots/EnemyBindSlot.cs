using UnityEngine;

public class EnemyBindSlot : MonoBehaviour
{
    public EnemyManager enemyManager;
     public bool occupied = false;
    public bool disabled = false;
    [HideInInspector] public Card boundCard;
    void Start()
    {
        enemyManager.BoundSlots.Add(this.gameObject);
        enemyManager.SortBoundSlots();
    }


    void Update()
    {
        if(transform.childCount > 0) { occupied = true; } else { occupied = false; }
    }
}
