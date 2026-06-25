using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Statistik")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("Opsi Kematian")]
    public bool hancurSaatMati = true; // TOGGLE: Centang jika musuh bisa mati/hilang
    public GameObject efekLedakanPrefab; // Tarik Prefab Ledakan ke sini

    [Header("UI")]
    public GameObject damagePopupPrefab; 
    private DamagePopup currentPopup;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Munculkan angka damage
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        if (currentPopup == null)
        {
            GameObject go = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
            currentPopup = go.GetComponent<DamagePopup>();
            currentPopup.Setup(amount, spawnPos);
        }
        else
        {
            currentPopup.AddDamage(amount, spawnPos);
        }

        // Cek jika darah habis
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Munculkan ledakan jika prefab sudah dimasukkan
        if (efekLedakanPrefab != null)
        {
            Instantiate(efekLedakanPrefab, transform.position, Quaternion.identity);
        }

        // 2. Cek apakah musuh ini boleh dihancurkan (Toggle)
        if (hancurSaatMati)
        {
            Destroy(gameObject);
        }
        else
        {
            // Jika tidak boleh hancur, kita buat dia "mati" secara logika saja
            isDead = true;
            Debug.Log(gameObject.name + " sudah 0 HP tapi tetap di tempat.");

            GetComponent<Collider2D>().enabled = false;
        }
    }
}