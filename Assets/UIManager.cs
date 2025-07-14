using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private Label playerHealthText;
    private VisualElement playerHealthBar;
    private VisualElement playerHealthBG;
    private Label manaText;
    private VisualElement opponentHealthBar;
    private Label opponentHealthText;

    private Button settingsButton;
    private Button quitButton;

    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;

    private Dictionary<VisualElement, Coroutine> activeEffects = new();

    private float previousHealth;
    private float displayedHealth;
    private float displayedHealthBarWidth;
    private float displayedMana;

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
    }

    private void OnEnable()
    {
        foreach (var label in root.Query<Label>().ToList())
        {
            Debug.Log("Found label: " + label.name);
        }

        playerHealthText = root.Q<Label>("PlayerHealthText");
        playerHealthBar = root.Q<VisualElement>("PlayerHealthBar");
        playerHealthBG = root.Q<VisualElement>("PlayerHealthBG");
        manaText = root.Q<Label>("ManaText");
        opponentHealthBar = root.Q<VisualElement>("OpponentHealthBar");
        opponentHealthText = root.Q<Label>("OpponentHealthText");

        settingsButton = root.Q<Button>("Settings");
        settingsButton.clicked += OnSettingsButtonClicked;

        quitButton = root.Q<Button>("QuitGame");
        quitButton.clicked += OnQuitButtonClicked;

        // Initialize displayed values
        displayedHealth = PlayerValueManager.Health;
        displayedHealthBarWidth = PlayerValueManager.Health / PlayerValueManager.MaxHealth;
        displayedMana = PlayerValueManager.Mana;
        previousHealth = PlayerValueManager.Health;
    }

    private void Update()
    {
        float currentHealth = PlayerValueManager.Health;
        float maxHealth = PlayerValueManager.MaxHealth;
        float currentMana = PlayerValueManager.Mana;

        // Trigger visual effect on damage
        if (currentHealth < previousHealth)
        {
            TriggerEffect(playerHealthBG);
            TriggerEffect(playerHealthBar);
        }

        // Smoothly interpolate values
        displayedHealth = Mathf.Lerp(displayedHealth, currentHealth, Time.deltaTime * 10f);
        displayedMana = Mathf.Lerp(displayedMana, currentMana, Time.deltaTime * 10f);
        float targetWidth = Mathf.Clamp01(currentHealth / maxHealth);
        displayedHealthBarWidth = Mathf.Lerp(displayedHealthBarWidth, targetWidth, Time.deltaTime * 10f);

        UpdateHealthUI(maxHealth);
        UpdateManaUI();

        previousHealth = currentHealth;
    }

    private void UpdateHealthUI(float maxHealth)
    {
        playerHealthText.text = $"{Mathf.RoundToInt(displayedHealth)}/{Mathf.RoundToInt(maxHealth)}";
        playerHealthBar.style.width = new Length(displayedHealthBarWidth * 103f, LengthUnit.Percent);
    }

    private void UpdateManaUI()
    {
        manaText.text = $"{Mathf.RoundToInt(displayedMana)}";
    }

    public void TriggerEffect(VisualElement target)
    {
        if (activeEffects.ContainsKey(target)) return;

        Coroutine routine = StartCoroutine(FlashAndShake(target));
        activeEffects[target] = routine;
    }

    private IEnumerator FlashAndShake(VisualElement target)
    {
        Color originalBackground = target.resolvedStyle.backgroundColor;
        target.style.backgroundColor = Color.white;

        bool hasBorder = target.resolvedStyle.borderTopWidth > 0 ||
                         target.resolvedStyle.borderRightWidth > 0 ||
                         target.resolvedStyle.borderBottomWidth > 0 ||
                         target.resolvedStyle.borderLeftWidth > 0;

        Color originalBorderTop = target.resolvedStyle.borderTopColor;
        Color originalBorderRight = target.resolvedStyle.borderRightColor;
        Color originalBorderBottom = target.resolvedStyle.borderBottomColor;
        Color originalBorderLeft = target.resolvedStyle.borderLeftColor;

        if (hasBorder)
        {
            target.style.borderTopColor = Color.white;
            target.style.borderRightColor = Color.white;
            target.style.borderBottomColor = Color.white;
            target.style.borderLeftColor = Color.white;
        }

        yield return new WaitForSeconds(0.1f);

        target.style.backgroundColor = originalBackground;

        if (hasBorder)
        {
            target.style.borderTopColor = originalBorderTop;
            target.style.borderRightColor = originalBorderRight;
            target.style.borderBottomColor = originalBorderBottom;
            target.style.borderLeftColor = originalBorderLeft;
        }

        Vector3 originalPos = target.transform.position;
        float duration = 0.25f;
        float time = 0f;
        float strength = 3f;

        while (time < duration)
        {
            float offsetX = Mathf.Sin(time * 50f) * strength;
            target.transform.position = originalPos + new Vector3(offsetX, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }

        target.transform.position = originalPos;
        activeEffects.Remove(target);
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings button has been pressed!");
    }

    private void OnQuitButtonClicked()
    {
        Debug.Log("Quit button has been pressed!");
    }
}
