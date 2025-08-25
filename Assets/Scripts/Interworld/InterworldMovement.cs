using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class InterworldMovement : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 2.5f;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 targetVelocity = new Vector2(inputX, inputY).normalized * maxSpeed;

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, acceleration * Time.deltaTime);
        currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, targetVelocity.y, acceleration * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = currentVelocity;
    }
}
