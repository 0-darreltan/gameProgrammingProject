using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    [Header("Referensi UI")]
    public GameObject notepadUI;    // Gambar catatan besar di layar
    public GameObject Hint_Icon;    // Segitiga/Icon di atas kepala player

    private bool isPlayerNearby = false;
    private bool isReading = false;

    void Start()
    {
        // Pastikan semua tertutup saat awal game
        if (notepadUI != null) notepadUI.SetActive(false);
        if (Hint_Icon != null) Hint_Icon.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            // Jika menekan tombol F
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isReading)
                {
                    TutupCatatan();
                }
                else
                {
                    BukaCatatan();
                }
            }
        }
    }

    void BukaCatatan()
    {
        isReading = true;
        if (notepadUI != null) notepadUI.SetActive(true);
        if (Hint_Icon != null) Hint_Icon.SetActive(false); // Sembunyikan ikon saat baca
        
        Time.timeScale = 0f; // Freeze game
        
        // Munculkan kursor agar bisa klik (jika ada tombol close di notepad)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TutupCatatan()
    {
        isReading = false;
        if (notepadUI != null) notepadUI.SetActive(false);
        if (Hint_Icon != null) Hint_Icon.SetActive(true); // Munculkan kembali ikon F
        
        Time.timeScale = 1f; // Jalan lagi
        
        // Sembunyikan kursor lagi (opsional)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Deteksi Player Masuk Area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (Hint_Icon != null) Hint_Icon.SetActive(true); 
        }
    }

    // Deteksi Player Keluar Area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (isReading) TutupCatatan(); // Otomatis tutup jika jalan menjauh
            
            if (Hint_Icon != null) Hint_Icon.SetActive(false);
        }
    }
}