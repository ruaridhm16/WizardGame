using UnityEngine;

public static class PlayerValueManager
{
    private static float health = 30;
    public static float MaxHealth = 30;

    public static int handDrawSize = 7;

    private static float mana = 10;

    private static float manaRegen = 2;

    public static float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
            if (health <= 0)
            {
                Debug.Log("Player Died");

            }
        }
    }

    public static float Mana
    {
        get => mana;
        set
        {
            mana = Mathf.Max(0, value); // No upper bound unless you define one
        }
    }

    public static float ManaRegen
    {
        get => manaRegen;
        set
        {
            manaRegen = Mathf.Max(0, value);
        }
    }

    public static void healPlayer(int healAmount)
    {
        health += healAmount;
    }

    public static void gainMana(int manaAmmount)
    {
        mana += manaAmmount;
    }

    public static void DamagePlayer(int damageAmount)
    {
        health -= damageAmount;
    }
}

