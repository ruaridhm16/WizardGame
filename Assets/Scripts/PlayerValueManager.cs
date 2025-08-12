using UnityEngine;

public static class PlayerValueManager
{
    private static float health = 100;
    public static float MaxHealth = 100;

    private static float mana = 100;

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
}
