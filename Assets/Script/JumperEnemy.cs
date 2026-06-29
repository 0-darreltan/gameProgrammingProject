using UnityEngine;

public class JumperEnemy : EnemyAI
{
    [Header("Jump Settings")]
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;

    // Kita override (timpa) fungsi RoamBehavior untuk perilaku khusus
   protected override void Start()
    {
        base.Start(); // Memanggil Start dari EnemyAI
        rb = GetComponent<Rigidbody2D>();
    }

    // Tambahkan logika lompat saat berjalan
    // Anda bisa memanggil ini di dalam Update atau saat musuh bergerak
    public void PerformJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Pastikan lantai Anda memiliki tag "Ground"
        {
            isGrounded = true;
        }
    }
}