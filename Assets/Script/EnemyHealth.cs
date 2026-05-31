using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public GameObject damagePopupPrefab; // Tarik Prefab DamageCanvas ke sini
    private DamagePopup currentPopup;    // Menyimpan angka yang sedang muncul

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Tentukan posisi di atas kepala musuh
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;

        if (currentPopup == null)
        {
            GameObject go = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
            currentPopup = go.GetComponent<DamagePopup>();
            currentPopup.Setup(amount, spawnPos);
        }
        else
        {
            // Kirim posisi terbaru musuh saat ini agar posisi angka di-reset ke sana
            currentPopup.AddDamage(amount, spawnPos);
        }

        if (currentHealth <= 0) Die();
    }

    void Die() { Destroy(gameObject); }
}