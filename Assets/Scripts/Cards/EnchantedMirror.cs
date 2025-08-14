using UnityEngine;

[CreateAssetMenu(fileName = "EnchantedMirror", menuName = "Cards/EnchantedMirror")]
public class EnchantedMirror : Card
{
    public int damageReflectPercentage = 50;

    public override void OnCast()
    {
        Debug.Log($"Reflected {damageReflectPercentage}% of damage!");
        // Your actual damage logic here
    }
}
