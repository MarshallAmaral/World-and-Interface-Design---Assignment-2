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

    private void Update()
    {
        anim.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));

        transform.localScale = new Vector3(Mathf.Sign(rb.linearVelocity.x), transform.localScale.y, 1);
    }

    private void FixedUpdate()
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
        if (context.performed)
        {
            targetPos = gameCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isMoving = true;

            GameObject highlight = Instantiate(ClickHighlight, targetPos, Quaternion.identity);
            Destroy(highlight, 0.7f);
        }
    }
}
