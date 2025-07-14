using UnityEngine;

[ExecuteAlways] // So values can be tested both in edit and play mode if desired
public class DebugValueChanger : MonoBehaviour
{
    [Header("Player Stats")]
    [Range(0, 100)] public float health = 100;
    [Range(0, 100)] public float maxHealth = 100;
    [Range(0, 100)] public float mana = 100;
    [Range(1, 20)] public int handSize = 10;

    [Header("Auto-Apply - updates PlayerValueManager every frame in play mode")]
    public bool autoApply = true;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
        if (autoApply) ApplyValuesToManager();
#endif
    }

    private void Update()
    {
        if (!Application.isPlaying) return;
        if (autoApply) ApplyValuesToManager();
    }

    public void ApplyValuesToManager()
    {
        PlayerValueManager.MaxHealth = maxHealth;
        PlayerValueManager.Health = health;
        PlayerValueManager.Mana = mana;
        PlayerValueManager.HandSize = handSize;
    }

    public void RefreshFromManager()
    {
        // Optional reverse sync (not automatic)
        health = PlayerValueManager.Health;
        maxHealth = PlayerValueManager.MaxHealth;
        mana = PlayerValueManager.Mana;
        handSize = PlayerValueManager.HandSize;
    }
}
