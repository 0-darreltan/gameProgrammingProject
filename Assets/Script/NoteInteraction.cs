using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    public GameObject notepadUI;    // Tarik 'NotepadUI' ke sini
    public GameObject hintF;        // Tarik 'TombolHint' (anak player) ke sini
    
    private bool isPlayerNearby = false;
    private bool isReading = false;

    void Start()
    {
        // This ensures the notepad is hidden when the game first loads
        if (notepadUI != null)
        {
            notepadUI.SetActive(false);
        }
    }
    void Update()
    {
        if (isPlayerNearby)
        {
            // Jika tekan F
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
        notepadUI.SetActive(true);
        hintF.SetActive(false); // Sembunyikan huruf F saat membaca
        Time.timeScale = 0f;    // Freeze game agar fokus membaca
    }

    void TutupCatatan()
    {
        isReading = false;
        notepadUI.SetActive(false);
        hintF.SetActive(true);  // Munculkan lagi huruf F
        Time.timeScale = 1f;    // Jalan lagi gamenya
    }

    // Deteksi masuk area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            hintF.SetActive(true); // Munculkan huruf F di atas kepala
        }
    }

    // Deteksi keluar area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            isReading = false;
            hintF.SetActive(false); // Hilangkan huruf F
            notepadUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}