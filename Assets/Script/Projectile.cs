using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    public int damage = 1;
    public float lifeTime = 2f; // Peluru hilang setelah 2 detik jika tidak kena apa-apa

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Kita cari objek Player di dalam game
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Ambil angka Scale X dari Player (1 atau -1)
            float direction = player.transform.localScale.x;
            
            // Kalikan kecepatan dengan arah hadap (positif ke kanan, negatif ke kiri)
            rb.linearVelocity = new Vector2(direction * speed, 0);

            // Putar gambar peluru agar menghadap ke kiri jika perlu
            if (direction < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
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