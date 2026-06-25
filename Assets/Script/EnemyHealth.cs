using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Statistik")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("Identitas Unik (WAJIB ISI)")]
    public string enemyID; 
    public bool simpanStatusKematian = true; 

    [Header("Opsi Kematian")]
    public bool hancurSaatMati = true; 
    public GameObject efekLedakanPrefab;

    [Header("UI Damage")]
    public GameObject damagePopupPrefab; // Tarik Prefab DamageCanvas ke sini
    private DamagePopup currentPopup;

    private bool isDead = false;

    void Start()
    {
        // CEK APAKAH MUSUH INI SUDAH PERNAH MATI?
        if (simpanStatusKematian && !string.IsNullOrEmpty(enemyID) && GlobalData.daftarMusuhMati.Contains(enemyID))
        {
            if (hancurSaatMati)
            {
                Destroy(gameObject);
            }
            else
            {
                SetSebagaiBangkai();
            }
            return;
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;

        // --- BAGIAN INI YANG TADI HILANG (LOGIKA DAMAGE POPUP) ---
        if (damagePopupPrefab != null)
        {
            // Tambahkan -1f pada sumbu Z agar angka muncul di DEPAN gambar map
            Vector3 spawnPos = transform.position + new Vector3(0, 1.2f, -1f);

            if (currentPopup == null)
            {
                GameObject go = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
                currentPopup = go.GetComponent<DamagePopup>();
                if (currentPopup != null) currentPopup.Setup(amount, spawnPos);
            }
            else
            {
                currentPopup.AddDamage(amount, spawnPos);
            }
        }
        // -------------------------------------------------------

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;

        if (simpanStatusKematian && !string.IsNullOrEmpty(enemyID))
        {
            if (!GlobalData.daftarMusuhMati.Contains(enemyID))
            {
                GlobalData.daftarMusuhMati.Add(enemyID);
            }
        }

        if (efekLedakanPrefab != null) Instantiate(efekLedakanPrefab, transform.position, Quaternion.identity);

        if (hancurSaatMati)
        {
            Destroy(gameObject);
        }
        else
        {
            SetSebagaiBangkai();
        }
    }

    void SetSebagaiBangkai()
    {
        isDead = true;
        this.enabled = false; 
        if (GetComponent<MiniBossCombat>() != null) GetComponent<MiniBossCombat>().enabled = false;
        
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols) c.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Static;

        GetComponent<SpriteRenderer>().color = Color.gray;
    }
    
    public int GetCurrentHealth() { return currentHealth; }
}