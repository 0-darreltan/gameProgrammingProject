using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float floatSpeed = 1f; 
    public float lifeTime = 1.2f; 
    
    private int totalDamage = 0;
    private float timer;
    private float currentYOffset; // Mengatur tinggi melayang dari 0
    private Vector3 basePosition; // Posisi dasar (kepala musuh)

    public void Setup(int damage, Vector3 startPos)
    {
        basePosition = startPos;
        totalDamage = damage;
        currentYOffset = 0; // Mulai dari 0
        UpdateText();
        timer = lifeTime;
    }

    public void AddDamage(int damage, Vector3 newEnemyPos)
    {
        totalDamage += damage;
        basePosition = newEnemyPos; // Update posisi jika musuh bergerak
        currentYOffset = 0; // RESET POSISI KE BAWAH LAGI
        UpdateText();
        timer = lifeTime; // Reset waktu
        
        // Efek "Pop" saat stacking
        transform.localScale = new Vector3(0.007f, 0.007f, 1f); // Sedikit membesar
    }

    void UpdateText()
    {
        textMesh.text = totalDamage.ToString();
    }

    void Update()
    {
        // Hitung ketinggian melayang
        currentYOffset += floatSpeed * Time.deltaTime;
        
        // Atur posisi: Posisi Dasar + Tinggi Melayang
        transform.position = basePosition + new Vector3(0, currentYOffset, 0);

        // Agar tulisan tidak terbalik saat musuh flip
        transform.rotation = Quaternion.identity;

        // Timer untuk menghilang
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}