using UnityEngine;

[CreateAssetMenu(fileName = "DarkOrb", menuName = "Cards/DarkOrb")]
public class DarkOrb : Card
{
    public int damage = 5;

    public override void OnActivation()
    {
        Debug.Log($"DarkOrd activated dealing {damage} damage!");
        // Your actual damage logic here
    }
}
