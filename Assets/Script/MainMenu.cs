using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel; // Taruh objek Panel Options di sini nanti jika ada

    // 1. FUNGSI UNTUK TOMBOL START
    public void StartGame()
    {
        // Langsung memuat scene Tutorial1 seperti yang diinginkan
        SceneManager.LoadScene("Tutorial1");
    }

    // 2. FUNGSI UNTUK TOMBOL OPTIONS
    public void OpenOptions()
    {
        Debug.Log("Membuka Menu Pengaturan/Options");
        
        // Logika dasar: Jika kamu membuat Panel khusus Options, 
        // kodingan ini akan otomatis memunculkan panel tersebut.
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    // Fungsi tambahan untuk menutup Options (jika ada tombol 'Back' di menu options)
    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    // 3. FUNGSI UNTUK TOMBOL EXIT
    public void ExitGame()
    {
        Debug.Log("Game Keluar!");
        Application.Quit(); // Menutup game (berfungsi setelah game di-build)
    }
}
