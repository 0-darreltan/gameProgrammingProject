using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifeTime = 2f; // Peluru hilang setelah 2 detik jika tidak kena apa-apa

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Memberi kecepatan pada peluru saat muncul
        rb.linearVelocity = transform.right * speed;
        
        // Hancurkan otomatis setelah beberapa detik agar tidak memenuhi memori
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah peluru mengenai Musuh
        if (collision.CompareTag("Enemy"))
        {
            // Beri damage ke musuh (kita buat script musuhnya nanti)
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            
            // Hancurkan peluru setelah kena
            Destroy(gameObject);
        }
    }
}