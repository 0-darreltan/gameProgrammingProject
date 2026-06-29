using UnityEngine;

public class JumperEnemy : EnemyAI
{
    [Header("Jump & Explode Settings")]
    public float jumpForce = 5f;
    public float explosionRange = 1.0f;
    public int explosionDamage = 10;
    
    private bool isGrounded;

    protected override void Start()
    {
        base.Start();
    }

    // Fungsi untuk melompat
    public void PerformJump()
    {
        if (isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // Musuh tidak lagi di tanah setelah melompat
        }
    }

    // Mengganti fungsi Attack() dari EnemyAI dengan logika ledakan
    // Catatan: Pastikan di EnemyAI fungsi Attack() ditulis 'protected virtual' 
    // agar bisa di-override
    protected override void Attack() 
    {
        Explode();
    }

    void Explode()
    {
        Debug.Log("JumperEnemy meledak!");
        
        // Cek apakah pemain ada di jangkauan ledakan
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= explosionRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
            }
        }

        // Hancurkan musuh setelah meledak
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Deteksi lantai untuk memungkinkan lompatan
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}