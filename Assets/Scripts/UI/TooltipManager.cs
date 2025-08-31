using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement tooltipPanel;

    [HideInInspector] public bool tooltipEnabled;

    [HideInInspector] public GameObject lastClicked = null;

    private Label spellName;
    private Label spellDescription;
    private Label castDescription;
    private Label bindDescription;
    private Label manaCost;




    private void Start()
    {
        root = uiDocument.rootVisualElement;
        tooltipPanel = root.Q<VisualElement>("TooltipPanel");
        if (tooltipPanel != null)
        {
            tooltipPanel.style.position = Position.Absolute;
            tooltipPanel.style.display = DisplayStyle.None;
            tooltipEnabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && tooltipEnabled)
        {
            DespawnTooltip();
        }
    }

    public void SpawnTooltip(Vector2 worldPos, Card card)
    {
        lastClicked = card.spawnedCard;
        tooltipEnabled = true;
        // Display tooltipPanel
        tooltipPanel.style.display = DisplayStyle.Flex;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(root.panel, screenPos);

        float panelW = root.resolvedStyle.width > 0 ? root.resolvedStyle.width : root.worldBound.width;

        float rightOffset = Mathf.Max(0f, panelW - panelPos.x);
        tooltipPanel.style.left = StyleKeyword.Auto;
        tooltipPanel.style.top = StyleKeyword.Auto;

        // Position with offsets
        tooltipPanel.style.right = rightOffset;
        tooltipPanel.style.bottom = 250;

        ChangeText(card);
    }

    public void DespawnTooltip()
    {
        // Hide tooltipPanel
        tooltipEnabled = false;
        tooltipPanel.style.display = DisplayStyle.None;
    }

    private void ChangeText(Card card)
    {
        spellName = root.Q<Label>("SpellName");
        spellDescription = root.Q<Label>("SpellDescription");
        castDescription = root.Q<Label>("CastDescription");
        bindDescription = root.Q<Label>("BindDescription");
        manaCost = root.Q<Label>("ManaCost");
        VisualElement line = root.Q<VisualElement>("LineChange");

        bindDescription.style.display =
            (card.bindDescription.Trim() == "On Bind: No effect.") ? DisplayStyle.None : DisplayStyle.Flex;

        castDescription.style.display =
            (card.castDescription.Trim() == "On Cast: No effect.") ? DisplayStyle.None : DisplayStyle.Flex;

        // Line is only visible if BOTH cast + bind descriptions are visible
        line.style.display =
            (castDescription.style.display == DisplayStyle.Flex &&
             bindDescription.style.display == DisplayStyle.Flex)
            ? DisplayStyle.Flex
            : DisplayStyle.None;

        spellName.text = card.cardName;
        spellDescription.text = card.cardDescription;
        castDescription.text = card.castDescription;
        bindDescription.text = card.bindDescription;
        manaCost.text = card.manaCost.ToString();
    }

}
