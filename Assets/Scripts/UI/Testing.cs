using UnityEditor.Tilemaps;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform PopUpPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PopUp.Create(new Vector3(0.8f, -1.32f, 0), "-20", PopUp.PopUpType.Damage);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PopUp.Create(new Vector3(0.8f, -1.32f, 0), "-5 Mana", PopUp.PopUpType.Mana);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PopUp.Create(new Vector3(2.15f, -1.22f, 0), "Enemy Skipped", PopUp.PopUpType.Header);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PopUp.Create(new Vector3(3.5f, -1.32f, 0), "+2 Cards", PopUp.PopUpType.Reward);
        }
    }
}
