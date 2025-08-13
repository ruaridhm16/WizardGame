using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
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

    // Death UI
    private VisualElement gameOverPanel;   // UXML: name = "DeathPanel" (class: death-panel)
    private Button retryButton;            // UXML: name = "Retry"

    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;

    private Dictionary<VisualElement, Coroutine> activeEffects = new();

    private float previousHealth;
    private float displayedHealth;
    private float displayedHealthBarWidth;
    private float displayedMana;

    private bool gameOverShown = false;

    private void Awake()
    {
        root = uiDocument.rootVisualElement; // get root from UIDocument
    }

    private void OnEnable()
    {
        foreach (var label in root.Query<Label>().ToList())
        {
            //Debug.Log("Found label: " + label.name);
        }

        playerHealthText = root.Q<Label>("PlayerHealthText");
        playerHealthBar = root.Q<VisualElement>("PlayerHealthBar");
        playerHealthBG = root.Q<VisualElement>("PlayerHealthBG");
        manaText = root.Q<Label>("ManaText");
        opponentHealthBar = root.Q<VisualElement>("OpponentHealthBar");
        opponentHealthText = root.Q<Label>("OpponentHealthText");

        settingsButton = root.Q<Button>("Settings");
        if (settingsButton != null) settingsButton.clicked += OnSettingsButtonClicked;

        quitButton = root.Q<Button>("QuitGame");
        if (quitButton != null) quitButton.clicked += OnQuitButtonClicked;

        // Death panel + retry
        gameOverPanel = root.Q<VisualElement>("DeathPanel");
        retryButton = root.Q<Button>("Retry");

        if (gameOverPanel != null)
        {
            // Start hidden and at opacity 0 (USS .death-panel handles opacity)
            gameOverPanel.style.display = DisplayStyle.None; // hidden from layout
            gameOverPanel.RemoveFromClassList("show");       // ensure starts at opacity 0
            gameOverPanel.pickingMode = PickingMode.Ignore;  // not clickable while hidden
        }

        if (retryButton != null)
        {
            retryButton.clicked -= OnRetryClicked;
            retryButton.clicked += OnRetryClicked;
        }

        // Initialize displayed values
        displayedHealth = PlayerValueManager.Health;
        displayedHealthBarWidth = PlayerValueManager.MaxHealth > 0
            ? PlayerValueManager.Health / PlayerValueManager.MaxHealth
            : 0f;
        displayedMana = PlayerValueManager.Mana;
        previousHealth = PlayerValueManager.Health;
        gameOverShown = false;
    }

    private void Update()
    {
        float currentHealth = PlayerValueManager.Health;
        float maxHealth = PlayerValueManager.MaxHealth;
        float currentMana = PlayerValueManager.Mana;

        // Trigger visual effect on damage
        if (!gameOverShown && currentHealth < previousHealth)
        {
            if (playerHealthBG != null) TriggerEffect(playerHealthBG);
            if (playerHealthBar != null) TriggerEffect(playerHealthBar);
        }

        // Smoothly interpolate values
        displayedHealth = Mathf.Lerp(displayedHealth, currentHealth, Time.deltaTime * 10f);
        displayedMana = Mathf.Lerp(displayedMana, currentMana, Time.deltaTime * 10f);
        float targetWidth = Mathf.Clamp01(maxHealth > 0 ? currentHealth / maxHealth : 0f);
        displayedHealthBarWidth = Mathf.Lerp(displayedHealthBarWidth, targetWidth, Time.deltaTime * 10f);

        UpdateHealthUI(maxHealth);
        UpdateManaUI();

        // Show death panel with fade
        if (!gameOverShown && currentHealth <= 0f)
        {
            ShowGameOver();
        }

        previousHealth = currentHealth;
    }

    private void ShowGameOver()
    {
        gameOverShown = true;

        if (gameOverPanel == null) return;

        // 1) Put it in layout (still at opacity 0 due to missing 'show' class)
        gameOverPanel.style.display = DisplayStyle.Flex;     // visible in layout
        gameOverPanel.pickingMode = PickingMode.Ignore;      // ignore clicks until visible
        gameOverPanel.RemoveFromClassList("show");           // ensure start state

        // 2) Next frame, add 'show' class -> triggers opacity transition per USS
        gameOverPanel.schedule.Execute(() =>
        {
            gameOverPanel.AddToClassList("show");            // opacity animates to 1
            gameOverPanel.pickingMode = PickingMode.Position; // now interactable
        }).StartingIn(0);

        activeEffects.Clear();
    }

    private void UpdateHealthUI(float maxHealth)
    {
        if (playerHealthText != null)
            playerHealthText.text = $"{Mathf.RoundToInt(displayedHealth)}/{Mathf.RoundToInt(maxHealth)}";

        if (playerHealthBar != null)
            playerHealthBar.style.width = new Length(displayedHealthBarWidth * 103f, LengthUnit.Percent);
    }

    private void UpdateManaUI()
    {
        if (manaText != null)
            manaText.text = $"{Mathf.RoundToInt(displayedMana)}";
    }

    public void TriggerEffect(VisualElement target)
    {
        if (target == null || activeEffects.ContainsKey(target)) return;
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

    private void OnRetryClicked()
    {
        // Reset stats then reload scene
        PlayerValueManager.Health = PlayerValueManager.MaxHealth;
        PlayerValueManager.Mana = 0f;

        DeckManager.Hand.Clear();
        DeckManager.HandCards.Clear();
        DeckManager.Deck.Clear();

        Scene active = SceneManager.GetActiveScene();
        SceneManager.LoadScene(active.buildIndex);
    }
}
