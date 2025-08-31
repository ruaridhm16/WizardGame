using UnityEngine;
using UnityEngine.UIElements;

public class TestScript : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement tooltipPanel;

    void Start()
    {
        root = uiDocument.rootVisualElement;
        tooltipPanel = root.Q<VisualElement>("TooltipPanel");
        if (tooltipPanel != null)
        {
            tooltipPanel.style.position = Position.Absolute;
        }
    }

    void Update()
    {
        if (tooltipPanel == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            // Convert to panel space (handles scaling/DPI)
            Vector2 panelMouse = RuntimePanelUtils.ScreenToPanel(root.panel, Input.mousePosition);

            // Distance from panel's right edge to the cursor X
            float panelW = root.resolvedStyle.width > 0 ? root.resolvedStyle.width : root.worldBound.width;
            float rightOffset = Mathf.Max(0f, panelW - panelMouse.x);

            // Use right+bottom; clear left/top to avoid conflicts
            tooltipPanel.style.left = StyleKeyword.Auto;
            tooltipPanel.style.top = StyleKeyword.Auto;

            tooltipPanel.style.right = rightOffset; // aligns RIGHT edge with cursor
            tooltipPanel.style.bottom = 240;        // constant Y level
        }
    }
}
