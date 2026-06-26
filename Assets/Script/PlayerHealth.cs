using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Statistik")]
    public int maxHealth = 3;
    public float iFramesDuration = 2f; // Durasi kebal (detik)
    public int numberOfFlashes = 5;    // Berapa kali kedip

    private int currentHealth;
    private bool isDead = false;
    private bool isInvincible = false;

    private Animator anim;
    private SpriteRenderer spriteRend;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        // Jika sedang kebal atau sudah mati, abaikan damage
        if (isInvincible || isDead) return;

        currentHealth -= damage;
        Debug.Log("Player Kena Hit! Sisa HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Mulai mode kebal dan efek kedip
            StartCoroutine(InvincibilityRoutine());
        }
    }

    // --- EFEK KEDIP & MODE KEBAL ---
    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        
        // Loop untuk membuat efek kedip
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // 1. Buat transparan (Alpha = 0.5)
            spriteRend.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

            // 2. Buat muncul normal (Alpha = 1)
            spriteRend.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        isInvincible = false;
    }

    void Update()
    {
        // Kematian otomatis jika jatuh dari map (misalnya Y kurang dari -15)
        if (transform.position.y < -15f && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("die");
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        
        // Pastikan saat mati warna kembali normal
        spriteRend.color = new Color(1, 1, 1, 1f);

        Invoke("TriggerGameOver", 2f); // Tunggu animasi mati selesai sebelum Game Over
    }

    void TriggerGameOver()
    {
        var gameOver = FindObjectOfType<GameOverManager>();
        if (gameOver != null) 
        {
            gameOver.ShowGameOver();
        }
        else 
        {
            // Fallback jika tidak ada UI Game Over
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        
        // Reset animasi
        anim.Rebind();
        anim.Update(0f);
        
        var pm = GetComponent<PlayerMovement>();
        pm.enabled = true;
        
        // Teleport ke posisi aman terakhir
        transform.position = pm.lastSafePosition;
        
        // Berikan waktu kebal agar tidak mati berulang jika spawn dekat musuh
        StartCoroutine(InvincibilityRoutine());
    }
}