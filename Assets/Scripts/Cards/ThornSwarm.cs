using UnityEngine;

[CreateAssetMenu(fileName = "ThornSwarm", menuName = "Cards/ThornSwarm")]
public class ThornSwarm : Card
{
    public int damage = 3;

    public override void OnActivation()
    {
        Debug.Log($"ThornSwarm activated dealing {damage} damage!");
        // Your actual damage logic here
    }
}
