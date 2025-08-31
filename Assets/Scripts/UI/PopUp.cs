using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private float disappearTimerMax;
    private Color textColor;
    private bool useScaling;
    private bool useNoise;
    private float baseMoveYSpeed;
    private float swayStrength;

    public enum PopUpType
    {
        Damage,
        Mana,
        Header,
        Reward
    }

    public static PopUp Create(Vector3 position, string message, PopUpType type)
    {
        Transform popupTransform = Instantiate(GameAssets.i.PopUpPrefab, position, Quaternion.identity);
        PopUp popUp = popupTransform.GetComponent<PopUp>();
        popUp.Setup(message, type);

        return popUp;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string message, PopUpType type)
    {
        textMesh.SetText(message);

        // Defaults
        disappearTimerMax = 1f;
        transform.localScale = Vector3.one;

        // Customize based on type
        switch (type)
        {
            case PopUpType.Damage:
                textMesh.fontSize = 2f; // smallest
                textMesh.color = new Color(0.9150943f, 0.2719384f, 0.2719384f); // red
                useScaling = true;
                useNoise = true;
                baseMoveYSpeed = 0.8f;
                swayStrength = 0.05f;
                disappearTimerMax *= 0.5f;
                break;

            case PopUpType.Mana:
                textMesh.fontSize = 2f; // smallest
                textMesh.color = new Color(0.545f, 0.690f, 0.812f); // blue
                useScaling = true;
                useNoise = true;
                baseMoveYSpeed = 0.8f;
                swayStrength = 0.05f;
                disappearTimerMax *= 0.5f;
                break;

            case PopUpType.Header:
                textMesh.fontSize = 3f; // largest
                textMesh.color = Color.white; // white
                useScaling = false;
                useNoise = false;
                baseMoveYSpeed = 0.5f;
                swayStrength = 0f;
                break;

            case PopUpType.Reward:
                textMesh.fontSize = 2f; // smallest
                textMesh.color = new Color(1f, 1f, 0.7f); // yellow
                useScaling = false;
                useNoise = false;
                baseMoveYSpeed = 0.8f;
                swayStrength = 0f;
                disappearTimerMax *= 0.5f;
                break;
        }

        disappearTimer = disappearTimerMax;
        textColor = textMesh.color;
    }

    private void Update()
    {
        // Movement
        Vector3 moveVector = new Vector3(0, baseMoveYSpeed) * Time.deltaTime;
        if (useNoise)
        {
            float sway = Mathf.Sin(Time.time * 3f) * swayStrength * 0.01f; // smooth sway left-right
            moveVector.x += sway;
        }
        transform.position += moveVector;

        // Scaling (only for damage/mana)
        if (useScaling)
        {
            if (disappearTimer > disappearTimerMax * 0.5f)
            {
                transform.localScale += Vector3.one * 0.2f * Time.deltaTime;
            }
            else
            {
                transform.localScale -= Vector3.one * 0.2f * Time.deltaTime;
            }
        }

        // Timer countdown
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0f)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
