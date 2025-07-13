using UnityEngine;

public class CardInteractions : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 destination;
    private Vector3 dragOffset;
    private Vector3 mouseDownPosition;

    [Header("Config")]
    public float dragThreshold = 0.1f;

    void OnMouseDown()
    {
        mouseDownPosition = GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        Vector3 currentMouse = GetMouseWorldPosition();
        float distance = Vector3.Distance(mouseDownPosition, currentMouse);

        if (!isDragging && distance > dragThreshold)
        {
            // Begin dragging
            destination = transform.position;
            isDragging = true;
            dragOffset = transform.position - mouseDownPosition;
        }

        if (isDragging)
        {
            Vector3 newPos = currentMouse + dragOffset;
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            // Finish drag
            isDragging = false;

            // Optionally detect drop targets here
            RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPosition(), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("DropZone"))
            {
                Debug.Log("Dropped on valid target!");
            }
            else
            {
                transform.position = destination;
            }
        }
        else
        {
            // Mouse didn't move much — treat as a click
            HandleClick();
        }
    }
    
    void HandleClick()
    {
        Debug.Log("Card clicked without dragging!");
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
