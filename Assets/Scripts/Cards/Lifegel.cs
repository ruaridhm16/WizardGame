using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Lifegel")]
public class Lifegel : Card
{
    public int healAmount;

    public override void OnActivation()
    {
        Debug.Log($"{cardName} heals for {healAmount} HP!");
    }
}
