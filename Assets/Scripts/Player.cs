using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

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
}
