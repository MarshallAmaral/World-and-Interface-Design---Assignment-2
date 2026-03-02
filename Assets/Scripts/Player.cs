using Pathfinding;
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
    private AIPath path;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
    }

    private void Update() // Handle animation and sprite flipping
    {
        anim.SetBool("Moving", isMoving);

        transform.localScale = new(Mathf.Sign(targetPos.x - transform.position.x), transform.localScale.y, 1);
    }

    private void FixedUpdate() // Handle movement
    {
        if (isMoving)
        {
            path.maxSpeed = moveSpeed;
            // move the player
            path.destination = targetPos;
            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                path.destination = transform.position;
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
