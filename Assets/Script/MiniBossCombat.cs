using UnityEngine;

public class MiniBossCombat : MonoBehaviour
{
    [Header("Referensi")]
    public GameObject missilePrefab;
    private EnemyHealth healthScript;

    [Header("Pengaturan Tembakan")]
    public float normalFireRate = 2f;    // 1 peluru tiap 2 detik
    public float enragedFireRate = 0.5f; // 1 peluru tiap 0.5 detik
    private float nextFireTime;

    [Header("Area Spawn (Y Random)")]
    public float minYOffset = -2f; // Batas bawah spawn peluru
    public float maxYOffset = 2f;  // Batas atas spawn peluru
    public float spawnXOffset = -3f; // Muncul di depan badan boss

    void Start()
    {
        healthScript = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // Cek apakah sudah waktunya nembak
        if (Time.time >= nextFireTime)
        {
            ShootMissile();
            
            // Tentukan delay tembakan berikutnya berdasarkan sisa darah
            float currentHealthPercent = (float)healthScript.GetCurrentHealth() / healthScript.maxHealth;

            if (currentHealthPercent <= 0.3f) // Jika darah sisa 30% ke bawah
            {
                nextFireTime = Time.time + enragedFireRate;
                Debug.Log("BOSS MARAH! Tembakan dipercepat!");
            }
            else
            {
                nextFireTime = Time.time + normalFireRate;
            }
        }
    }

    void ShootMissile()
    {
        // Tentukan posisi Y secara acak
        float randomY = Random.Range(transform.position.y + minYOffset, transform.position.y + maxYOffset);
        Vector3 spawnPos = new Vector3(transform.position.x + spawnXOffset, randomY, 0);

        // Munculkan misil
        Instantiate(missilePrefab, spawnPos, Quaternion.identity);
    }

    // Untuk membantu melihat area random di editor (garis hijau)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 pos = transform.position + new Vector3(spawnXOffset, 0, 0);
        Gizmos.DrawLine(pos + Vector3.up * minYOffset, pos + Vector3.up * maxYOffset);
    }
}