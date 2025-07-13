using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragTest : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;

    private Vector3 lastPosition;
    private Vector3 velocity;

    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        // Optional: Adjust damping for smoother slowdown after release
        rb.gravityScale = 0;
        rb.linearDamping = 1f;
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3 newPosition = mousePos + offset;

            rb.MovePosition(newPosition);

            velocity = (newPosition - lastPosition) / Time.deltaTime;
            lastPosition = newPosition;
        }
    }

    void OnMouseDown()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        offset = transform.position - mousePos;
        dragging = true;

        rb.linearVelocity = Vector2.zero;
        lastPosition = transform.position;
    }

    void OnMouseUp()
    {
        dragging = false;

        rb.linearVelocity = new Vector2(velocity.x, velocity.y);
    }
}
