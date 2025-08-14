using UnityEngine;

[CreateAssetMenu(fileName = "ManaBerry", menuName = "Cards/ManaBerry")]
public class ManaBerry : Card
{
    public int manaGeneration = 5;

    public override void OnCast()
    {
        Debug.Log($"Generated {manaGeneration} mana!");
        // Your actual damage logic here
    }
}
