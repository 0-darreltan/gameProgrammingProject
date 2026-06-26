using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;

    private PlayerHealth playerHealth;

    private void Start()
    {
        // Sembunyikan panel saat game mulai
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
            
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Fungsi ini dipanggil dari PlayerHealth setelah animasi mati selesai
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            // Time.timeScale = 0f; // Aktifkan ini jika ingin game pause saat Game Over
        }
    }

    // 1. Fungsi untuk tombol RETRY
    public void Retry()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
            
        Time.timeScale = 1f;

        if (playerHealth != null)
        {
            playerHealth.Respawn();
        }
        else
        {
            // Fallback jika player tidak ditemukan (akan reload scene)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 2. Fungsi untuk tombol QUIT
    public void Quit()
    {
        Time.timeScale = 1f;
        // Pastikan scene menu utama bernama "Menu" dan sudah ada di Build Settings
        SceneManager.LoadScene("Menu"); 
    }
}
