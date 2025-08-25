using UnityEngine;

public class InterworldAnimation : MonoBehaviour
{
    public GameObject cloak;
    public GameObject head;

    private Rigidbody2D rb;

    private bool flipped;

    private float inputX;
    private float inputY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentScaleHead = head.transform.localScale.x;
        float currentScaleCloak = cloak.transform.localScale.x;

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // Head flipping
        if (rb.linearVelocityX > 0.01f && inputX > 0.01f)
        {
            head.transform.localScale = new Vector2 (Mathf.Abs(currentScaleHead), head.transform.localScale.y);
        }
        else if ((rb.linearVelocityX < -0.01f && inputX < -0.01f))
        {
            head.transform.localScale = new Vector2(-Mathf.Abs(currentScaleHead), head.transform.localScale.y);
        }

        // Cloak flipping
        if (rb.linearVelocityX > 1f && inputX > 0.01f)
        {
            flipped = false;
            cloak.transform.localScale = new Vector2(Mathf.Abs(currentScaleCloak), cloak.transform.localScale.y);
        }
        else if ((rb.linearVelocityX < -1f && inputX < -0.01f))
        {
            flipped = true;
            cloak.transform.localScale = new Vector2(-Mathf.Abs(currentScaleCloak), cloak.transform.localScale.y);
        }
    }
}
