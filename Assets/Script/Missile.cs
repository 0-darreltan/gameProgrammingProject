using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Settings Misil")]
    public float speed = 12f;
    public int damage = 1;
    public float lifeTime = 3f; // Misil hancur setelah 3 detik

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Pastikan tidak jatuh karena gravitasi
        if (rb != null) rb.gravityScale = 0; 

        // --- LOGIKA HANCUR OTOMATIS ---
        // Perintah ini akan menghapus objek ini setelah 'lifeTime' detik
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Gerakkan misil ke kiri secara paksa
        if (rb != null)
        {
            Vector2 position = rb.position;
            position.x -= speed * Time.fixedDeltaTime;
            rb.MovePosition(position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jika kena Player
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Hancur saat kena player
        }
        
        // Jika kena Lantai/Tembok
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Hancur saat kena lantai
        }
    }
}