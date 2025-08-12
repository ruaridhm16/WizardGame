using UnityEngine;

[ExecuteAlways] // So values can be tested both in edit and play mode if desired
public class DebugValueChanger : MonoBehaviour
{
    [Header("Player Stats")]
    [Range(0, 100)] public float health = 100;
    [Range(0, 100)] public float maxHealth = 100;
    [Range(0, 100)] public float mana = 100;


    [Header("Apply")]
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
        if (Input.GetKeyDown(KeyCode.Backspace)) InstaKill();
    }

    public void ApplyValuesToManager()
    {
        PlayerValueManager.MaxHealth = maxHealth;
        PlayerValueManager.Health = health;
        PlayerValueManager.Mana = mana;
    }

    public void RefreshFromManager()
    {
        //Reverse sync
        health = PlayerValueManager.Health;
        maxHealth = PlayerValueManager.MaxHealth;
        mana = PlayerValueManager.Mana;
    }

    public void InstaKill()
    {
        PlayerValueManager.Health = 0f;
    }
}
