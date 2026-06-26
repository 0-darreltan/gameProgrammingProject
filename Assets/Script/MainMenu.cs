using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel; // Taruh objek Panel Options di sini nanti jika ada
    public IntroCutscene introCutscene; // Tarik objek dengan skrip IntroCutscene jika menggunakan cutscene

    private void Awake()
    {
        // Awake bisa dibiarkan kosong atau dihapus, karena referensi introCutscene tidak lagi dibutuhkan di MainMenu
    }

    // 1. FUNGSI UNTUK TOMBOL START
    public void StartGame()
    {
        Debug.Log("MainMenu: Memuat scene CutScene.");
        SceneManager.LoadScene("CutScene");
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
