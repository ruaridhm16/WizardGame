using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Cards/Fireball")]
public class Fireball : Card
{
    public int damage = 5;

    public override void OnCast()
    {
        Debug.Log($"Fireball activated dealing {damage} damage!");
        // Your actual damage logic here
    }
}
