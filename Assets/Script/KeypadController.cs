using UnityEngine;
using TMPro; // Pastikan kamu sudah import TextMeshPro

public class KeypadController : MonoBehaviour
{
    [Header("Pengaturan Kode")]
    public string kodeBenar = "1234"; // Kamu bisa ganti angka ini di Inspector!
    
    [Header("Referensi UI & Objek")]
    public TextMeshProUGUI displayText; // Seret objek Text ke sini
    public DoorSystem pintuTarget;     // Seret objek Pintu ke sini
    
    private string inputUser = "";

    // Saat Keypad muncul, reset tulisan jadi kosong
    void OnEnable() 
    { 
        inputUser = ""; 
        UpdateTampilan(); 
    }

    // Fungsi ini dipanggil oleh kotak-kotak transparan tadi
    public void TekanAngka(string nomor)
    {
        Debug.Log("SAYA MENGKLIK ANGKA: " + nomor);
        // Batasi hanya bisa input 4 angka (atau sesuaikan dengan panjang kodeBenar)
        if (inputUser.Length < kodeBenar.Length) 
        {
            inputUser += nomor;
            UpdateTampilan();
        }

        // Cek apakah sudah benar
        if (inputUser == kodeBenar)
        {
            Berhasil();
        }
        else if (inputUser.Length >= kodeBenar.Length)
        {
            // Jika sudah penuh tapi salah, beri jeda sebentar lalu hapus
            Invoke("Hapus", 0.5f); 
        }
    }

    void UpdateTampilan() 
    { 
        if (displayText != null) displayText.text = inputUser; 
    }

    void Hapus() 
    { 
        inputUser = ""; 
        UpdateTampilan(); 
    }

    void Update()
    {
        // Jika tombol F ditekan saat keypad terbuka, maka tutup
        if (Input.GetKeyDown(KeyCode.F))
        {
            TutupKeypad();
        }
    }

    public void TutupKeypad()
    {
        inputUser = ""; // RESET PASSWORD saat ditutup
        UpdateTampilan();
        
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Jalan lagi duniaya
        
        // Sembunyikan kursor lagi (tergantung kebutuhan game kamu)
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Berhasil()
    {
        if (displayText != null) displayText.text = "OPEN";
        
        // Panggil fungsi buka di script pintu
        if (pintuTarget != null) pintuTarget.BukaPintu(); 
        
        Invoke("TutupKeypad", 1f);
    }
}