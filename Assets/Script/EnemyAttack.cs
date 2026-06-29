using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 2; // Jumlah HP yang dikurangi

    // Fungsi ini dipanggil otomatis oleh Unity saat sesuatu masuk ke area trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mengecek apakah yang kena trigger adalah objek dengan tag "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Kena");
            // Mengambil skrip PlayerHealth dari objek Player yang kena
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}