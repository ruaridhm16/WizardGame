using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public bool cardsSelected;
    public bool cardsDragging;
    public bool bindable = true;

    public int castCost;
    public int summonCost;
    public int bindCost;

    public bool castExpensive;
    public bool summonExpensive;
    public bool bindExpensive;

    private bool castable = true;
    private bool summonable = true;

    private Label playerHealthText;
    private VisualElement playerHealthBar;
    private VisualElement playerHealthBG;
    private Label manaText;
    private VisualElement opponentHealthBar;
    private Label opponentHealthText;

    private Button settingsButton;
    private Button quitButton;

    private VisualElement gameOverPanel;
    private Button retryButton;

    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;

    private Dictionary<VisualElement, Coroutine> activeEffects = new();

    private float previousHealth;
    private float displayedHealth;
    private float displayedHealthBarWidth;
    private float displayedMana;

    private bool gameOverShown = false;

    private Button castButton;
    private Button bindButton;
    private Button summonButton;

    private Label castCostLabel;
    private Label bindCostLabel;
    private Label summonCostLabel;

    private struct BtnSkin { public Color tint; public Color text; }
    private readonly Dictionary<Button, BtnSkin> original = new();
    private readonly Color disabledTint = new Color(0.33f, 0.29f, 0.25f, 1f);
    private readonly Color disabledText = new Color(0.63f, 0.58f, 0.52f, 1f);
    private const float disabledOpacity = 0.55f;

    public void SetCastable(bool v) { castable = v; }
    public void SetSummonable(bool v) { summonable = v; }

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
    }

    private void OnEnable()
    {
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

        gameOverPanel = root.Q<VisualElement>("DeathPanel");
        retryButton = root.Q<Button>("Retry");
        if (gameOverPanel != null)
        {
            gameOverPanel.style.display = DisplayStyle.None;
            gameOverPanel.RemoveFromClassList("show");
            gameOverPanel.pickingMode = PickingMode.Ignore;
        }
        if (retryButton != null)
        {
            retryButton.clicked -= OnRetryClicked;
            retryButton.clicked += OnRetryClicked;
        }

        displayedHealth = PlayerValueManager.Health;
        displayedHealthBarWidth = PlayerValueManager.MaxHealth > 0
            ? PlayerValueManager.Health / PlayerValueManager.MaxHealth : 0f;
        displayedMana = PlayerValueManager.Mana;
        previousHealth = PlayerValueManager.Health;
        gameOverShown = false;

        castButton = root.Q<Button>("CastButton");
        bindButton = root.Q<Button>("BindButton");
        summonButton = root.Q<Button>("SummonButton");

        castCostLabel = root.Q<Label>("CastCost");
        summonCostLabel = root.Q<Label>("SummonCost");
        bindCostLabel = root.Q<Label>("BindCost");

        WireActionButtons();
        CaptureAndLogColors();
        UpdateButtonsNow();
        UpdateCostsNow();
    }

    private void Update()
    {
        float currentHealth = PlayerValueManager.Health;
        float maxHealth = PlayerValueManager.MaxHealth;
        float currentMana = PlayerValueManager.Mana;

        if (!gameOverShown && currentHealth < previousHealth)
        {
            if (playerHealthBG != null) TriggerEffect(playerHealthBG);
            if (playerHealthBar != null) TriggerEffect(playerHealthBar);
        }

        displayedHealth = Mathf.Lerp(displayedHealth, currentHealth, Time.deltaTime * 10f);
        displayedMana = Mathf.Lerp(displayedMana, currentMana, Time.deltaTime * 10f);
        float targetWidth = Mathf.Clamp01(maxHealth > 0 ? currentHealth / maxHealth : 0f);
        displayedHealthBarWidth = Mathf.Lerp(displayedHealthBarWidth, targetWidth, Time.deltaTime * 10f);

        UpdateHealthUI(maxHealth);
        UpdateManaUI();

        if (!gameOverShown && currentHealth <= 0f) ShowGameOver();

        UpdateButtonsNow();
        UpdateCostsNow();
        previousHealth = currentHealth;
    }

    private void ShowGameOver()
    {
        gameOverShown = true;
        if (gameOverPanel == null) return;
        gameOverPanel.style.display = DisplayStyle.Flex;
        gameOverPanel.pickingMode = PickingMode.Ignore;
        gameOverPanel.RemoveFromClassList("show");
        gameOverPanel.schedule.Execute(() =>
        {
            gameOverPanel.AddToClassList("show");
            gameOverPanel.pickingMode = PickingMode.Position;
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
        Color originalBackground = target.resolvedStyle.unityBackgroundImageTintColor;
        target.style.unityBackgroundImageTintColor = Color.white;

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

        target.style.unityBackgroundImageTintColor = originalBackground;

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

    private void OnSettingsButtonClicked() { Debug.Log("Settings pressed"); }
    private void OnQuitButtonClicked() { Debug.Log("Quit pressed"); }
    private void OnRetryClicked()
    {
        PlayerValueManager.Health = PlayerValueManager.MaxHealth;
        PlayerValueManager.Mana = 0f;
        Scene active = SceneManager.GetActiveScene();
        SceneManager.LoadScene(active.buildIndex);
    }

    private void WireActionButtons()
    {
        if (castButton != null) { castButton.clicked -= OnCast; castButton.clicked += OnCast; }
        if (bindButton != null) { bindButton.clicked -= OnBind; bindButton.clicked += OnBind; }
        if (summonButton != null) { summonButton.clicked -= OnSummon; summonButton.clicked += OnSummon; }
    }

    private void CaptureAndLogColors()
    {
        CaptureOne(castButton, "Cast");
        CaptureOne(bindButton, "Bind");
        CaptureOne(summonButton, "Summon");
    }

    private void CaptureOne(Button b, string label)
    {
        if (b == null) return;
        var skin = new BtnSkin
        {
            tint = b.resolvedStyle.unityBackgroundImageTintColor,
            text = b.resolvedStyle.color
        };
        original[b] = skin;
    }

    private void UpdateButtonsNow()
    {
        if (castButton == null || bindButton == null || summonButton == null) return;

        if (cardsDragging)
        {
            Show(castButton, false);
            Show(bindButton, false);
            Show(summonButton, false);
            return;
        }

        if (cardsSelected)
        {
            Show(castButton, true);
            Show(bindButton, true);
            Show(summonButton, false);
        }
        else
        {
            Show(castButton, false);
            Show(bindButton, false);
            Show(summonButton, true);
        }

        if (bindExpensive) bindable = false;

        bool canCast = castable && !castExpensive;
        bool canBind = bindable && !bindExpensive;
        bool canSummon = summonable && !summonExpensive;

        SetDisabled(castButton, !canCast);
        SetDisabled(bindButton, !canBind);
        SetDisabled(summonButton, !canSummon);
    }

    private void UpdateCostsNow()
    {
        SetCost(castCostLabel, castCost, castExpensive);
        SetCost(summonCostLabel, summonCost, summonExpensive);
        SetCost(bindCostLabel, bindCost, bindExpensive);
    }

    private void Show(VisualElement ve, bool show)
    {
        ve.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void SetDisabled(Button b, bool disabled)
    {
        if (!original.ContainsKey(b)) return;
        if (disabled)
        {
            b.style.unityBackgroundImageTintColor = disabledTint;
            b.style.color = disabledText;
            b.style.opacity = disabledOpacity;
            b.pickingMode = PickingMode.Ignore;
            b.SetEnabled(false);
        }
        else
        {
            var skin = original[b];
            b.style.unityBackgroundImageTintColor = skin.tint;
            b.style.color = skin.text;
            b.style.opacity = 1f;
            b.pickingMode = PickingMode.Position;
            b.SetEnabled(true);
        }
    }

    private void SetCost(Label l, int value, bool expensive)
    {
        if (l == null) return;
        l.text = value.ToString();
        l.style.color = expensive ? Color.red : Color.white;
    }

    private void OnCast() { Debug.Log("Cast pressed"); }
    private void OnBind() { Debug.Log("Bind pressed"); }
    private void OnSummon() { Debug.Log("Summon pressed"); }
}
