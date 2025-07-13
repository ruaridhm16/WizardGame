using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Fireball")]
public class Fireball : Card
{
    public int damage;

    public override void OnActivation()
    {
        Debug.Log($"{cardName} deals {damage} damage!");
    }
}
