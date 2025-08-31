using UnityEngine;

public class GameAssets : MonoBehaviour
{
    // This is for prefabs, the script is a static instance
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }


    public Transform PopUpPrefab;


}

