using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 2; // Jumlah HP yang dikurangi
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    // Fungsi ini dipanggil otomatis oleh Unity saat sesuatu masuk ke area trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Mengecek apakah yang kena trigger adalah objek dengan tag "Player"
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Debug.Log("Player terkena damage!");
                
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    lastAttackTime = Time.time; // ✅ Catat waktu serangan
                }
            }
        }
    }
}