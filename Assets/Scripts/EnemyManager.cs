using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public int MaxHealth = 100;
    public int health = 100;
    public int mana = 10;
    public void DealDamage(int damage)
    {
        health -= damage;
    }

    public void EnemyAction()
    {

    }

    public void HealEnemy(int healAmount)
    {
        
    }
}
