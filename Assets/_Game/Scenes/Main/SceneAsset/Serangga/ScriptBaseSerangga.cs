using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed      = 5f;   // kecepatan puncak
    [SerializeField] private float acceleration  = 25f;  // percepatan (unit/s²)
    [SerializeField] private float deceleration  = 35f;  // perlambatan saat lepas tombol
    [SerializeField] private float jumpForce     = 400f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius  = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Enemy Jump Logic")]
    [SerializeField] private Transform gapCheck;
    [SerializeField] private Transform downhillCheck;
    [SerializeField] private Transform cliffCheck;
    [SerializeField] private float enemyJumpCooldown = 1f;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool isPlayer;
    private bool isGrounded;
    private float lastEnemyJumpTime = -999f;

    private void Awake()
    {
        rb      = GetComponent<Rigidbody2D>();
        isPlayer = CompareTag("Player");
    }

    private void Update()
    {
        /* --------- INPUT / AI --------- */
        float moveInput = isPlayer ? Input.GetAxisRaw("Horizontal") : 1f;     // AI = jalan terus
        float targetSpeed = moveInput * maxSpeed;

        /* --------- ACCELERATION --------- */
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float newVelX = Mathf.MoveTowards(rb.velocity.x, targetSpeed, accelRate * Time.deltaTime);
        rb.velocity   = new Vector2(newVelX, rb.velocity.y);

        /* --------- Flip sprite --------- */
        if (newVelX > 0.01f && !facingRight) Flip();
        if (newVelX < -0.01f && facingRight) Flip();

        /* --------- Grounded check --------- */
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        /* --------- Player Jump --------- */
        if (isPlayer && isGrounded && Input.GetButtonDown("Jump"))
            rb.AddForce(Vector2.up * jumpForce);

        /* --------- Enemy logic --------- */
        if (!isPlayer) HandleEnemyJump();
    }

    private void HandleEnemyJump()
    {
        bool noGroundAhead  = !Physics2D.OverlapCircle(gapCheck.position,      groundRadius, groundLayer);
        bool groundDownhill =  Physics2D.OverlapCircle(downhillCheck.position, groundRadius, groundLayer);
        bool obstacleAhead  =  Physics2D.OverlapCircle(cliffCheck.position,    groundRadius, groundLayer);

        bool shouldJump =
              (noGroundAhead && groundDownhill)   // turunan tajam
           || obstacleAhead;                      // penghalang

        if (isGrounded && shouldJump && Time.time - lastEnemyJumpTime >= enemyJumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce);
            lastEnemyJumpTime = Time.time;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    /* --------- Gizmos --------- */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (groundCheck)   Gizmos.DrawWireSphere(groundCheck.position,   groundRadius);
        if (gapCheck)      Gizmos.DrawWireSphere(gapCheck.position,      groundRadius);
        if (downhillCheck) Gizmos.DrawWireSphere(downhillCheck.position, groundRadius);
        if (cliffCheck)    Gizmos.DrawWireSphere(cliffCheck.position,    groundRadius);
    }
}
