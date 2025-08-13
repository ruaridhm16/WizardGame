using UnityEngine;
[CreateAssetMenu(fileName = "Lifegel", menuName = "Cards/Lifegel")]
public class Lifegel : Card
{
    public int healAmount = 5;

    public override void OnActivation()
    {
        Debug.Log($"Lifegel healed {healAmount} health!");
        // Your actual damage logic here
    }
}
