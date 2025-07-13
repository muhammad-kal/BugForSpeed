using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 400f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Enemy Jump Logic")]
    [SerializeField] private Transform gapCheck;        // ujung depan kaki
    [SerializeField] private Transform downhillCheck;   // sedikit lebih rendah
    [SerializeField] private Transform cliffCheck;      // sensor halangan

    [SerializeField] private float enemyJumpCooldown = 1f;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool isPlayer;
    private bool isGrounded;
    private float lastEnemyJumpTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isPlayer = CompareTag("Player");
        Debug.Log(isPlayer);
    }

    private void Update()
    {
        // float moveInput = isPlayer ? Input.GetAxisRaw("Horizontal") : 1f;
        float moveInput = isPlayer ? 1f : 1f;
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0 && !facingRight) Flip();
        if (moveInput < 0 && facingRight) Flip();

        // --- Grounded check (umum) ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isPlayer)
        {
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
            return;
        }

        // ---------- Enemy logic ----------
        bool noGroundAhead = !Physics2D.OverlapCircle(gapCheck.position, groundRadius, groundLayer);
        bool groundDownhill = Physics2D.OverlapCircle(downhillCheck.position, groundRadius, groundLayer);
        bool obstacleAhead = Physics2D.OverlapCircle(cliffCheck.position, groundRadius, groundLayer);

        bool shouldJump =
            (noGroundAhead && groundDownhill)   // turunan tajam → lompat
            || obstacleAhead;                   // ada penghalang depan → lompat

        if (isGrounded
            && shouldJump
            && Time.time - lastEnemyJumpTime >= enemyJumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce);
            lastEnemyJumpTime = Time.time;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ---------- Gizmos ----------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (groundCheck   != null) Gizmos.DrawWireSphere(groundCheck.position,   groundRadius);
        if (gapCheck      != null) Gizmos.DrawWireSphere(gapCheck.position,      groundRadius);
        if (downhillCheck != null) Gizmos.DrawWireSphere(downhillCheck.position, groundRadius);
        if (cliffCheck    != null) Gizmos.DrawWireSphere(cliffCheck.position,    groundRadius);
    }
}
