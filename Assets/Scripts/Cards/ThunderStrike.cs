using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrike", menuName = "Cards/ThunderStrike")]
public class ThunderStrike : Card
{
    public int damage = 8;

    public override void OnCast()
    {
        Debug.Log($"Fireball activated dealing {damage} damage!");
        // Your actual damage logic here
    }
}
