using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [Header("Referensi Map")]
    public GameObject mapTertutup; 
    public GameObject mapTerbuka;  
    
    [Header("Referensi Pindah Level")]
    public GameObject triggerPindahLevel; // Tarik objek 'TriggerLanjutStage3' ke sini

    [Header("UI")]
    public GameObject uiKeypad;     

    private bool diDekatPintu = false;
    private bool sudahTerbuka = false;

    public GameObject petunjukTeks; // Tarik objek teks "Tekan F" ke sini

    void Start() 
    { 
        uiKeypad.SetActive(false); 
        
        // Awal game: Map Tertutup Nyala, Map Terbuka & Trigger Pindah MATI
        mapTertutup.SetActive(true);
        mapTerbuka.SetActive(false);
        if(triggerPindahLevel != null) triggerPindahLevel.SetActive(false);
    }

    void Update()
    {
        // Jika player di dekat pintu, pintu belum terbuka, dan tekan F
        if (diDekatPintu && !sudahTerbuka && Input.GetKeyDown(KeyCode.F))
        {
            // Jika keypad sedang MATI, maka NYALAKAN
            if (!uiKeypad.activeSelf)
            {
                BukaKeypadUI();
            }
            // Catatan: logika menutup sudah diurus oleh KeypadController di atas
        }
    }

    void BukaKeypadUI()
    {
        uiKeypad.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }

    // Fungsi ini dipanggil oleh KeypadController saat kode BENAR
    public void BukaPintu()
    {
        sudahTerbuka = true;
        
        // 1. Tukar gambar map
        mapTertutup.SetActive(false); 
        mapTerbuka.SetActive(true);   
        
        // 2. NYALAKAN BOX COLLIDER PINDAH LEVEL
        if(triggerPindahLevel != null) triggerPindahLevel.SetActive(true); 
        
        Debug.Log("Pintu Terbuka & Tembok Pindah Level Aktif!");
    }

    // Deteksi Player di depan pintu
    // Script akan mendeteksi saat Player "Overlap" atau masuk ke dalam kotak
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            diDekatPintu = true;
            if(petunjukTeks != null) petunjukTeks.SetActive(true); // Munculkan tulisan
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            diDekatPintu = false;
            if(petunjukTeks != null) petunjukTeks.SetActive(false); // Hilangkan tulisan
        }
    }
}