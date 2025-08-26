using UnityEngine;

public class InterworldAnimation : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject cloak;
    public GameObject head;
    [Header("Sprites")]
    public Sprite headFront;
    public Sprite headBack;
    public Sprite cloakFront;
    public Sprite cloakBack;

    private Rigidbody2D rb;

    private bool flipped; //True if sprite is flipped because player is moving left
    private bool switched; //True if sprite is switched because player is moving up

    private float inputX;
    private float inputY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        switched = false;
        flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        HandleFlipping();
        HandleSwitching();
    }

    private void HandleFlipping()
    {
        float currentScaleHead = head.transform.localScale.x;
        float currentScaleCloak = cloak.transform.localScale.x;

        // Head flipping
        if (rb.linearVelocityX > 0.01f && inputX > 0.01f)
        {
            head.transform.localScale = new Vector2(Mathf.Abs(currentScaleHead), head.transform.localScale.y);
            cloak.transform.localScale = new Vector2(Mathf.Abs(currentScaleCloak), cloak.transform.localScale.y);
        }
        else if ((rb.linearVelocityX < -0.01f && inputX < -0.01f))
        {
            head.transform.localScale = new Vector2(-Mathf.Abs(currentScaleHead), head.transform.localScale.y);
            cloak.transform.localScale = new Vector2(-Mathf.Abs(currentScaleCloak), cloak.transform.localScale.y);
        }
    }

    private void HandleSwitching()
    {
        // Sprite Switching
        if (rb.linearVelocityY > 0.01f && inputY > 0.01f)
        {
            switched = true;
            head.GetComponent<SpriteRenderer>().sprite = headBack;
            cloak.GetComponent<SpriteRenderer>().sprite = cloakBack;
        }
        else if ((rb.linearVelocityY < 0.01f && inputY < 0.01f))
        {
            switched = false;
            head.GetComponent<SpriteRenderer>().sprite = headFront;
            cloak.GetComponent<SpriteRenderer>().sprite = cloakFront;
        }
    }
}
