using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private Camera gameCam;
    [SerializeField] private GameObject ClickHighlight;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isMoving;
    private Vector2 targetPos;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() // Handle animation and sprite flipping
    {
        anim.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));

        transform.localScale = new Vector3(Mathf.Sign(rb.linearVelocity.x), transform.localScale.y, 1);
    }

    private void FixedUpdate() // Handle movement
    {
        if (isMoving)
        {
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) // Only trigger on the initial click, not while holding
        {
            Vector3 mousePos = gameCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // This will damage the enemy and deal 5 damage 
                hit.collider.GetComponent<Enemy>().TakeDamage(5);
            }
            else
            {
                // Moves player
                targetPos = mousePos;
                isMoving = true;

                GameObject highlight = Instantiate(ClickHighlight, mousePos, Quaternion.identity);
                Destroy(highlight, 0.7f);
            }
        }
    }
}
