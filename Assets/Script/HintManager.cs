using System.Collections;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject hintPanel;
    public TextMeshProUGUI hintText;

    [Header("Hint Settings")]
    [Tooltip("Waktu tunggu (dalam menit) antar kemunculan hint.")]
    public float delayInMinutes = 2f;
    [Tooltip("Berapa lama (dalam detik) teks akan menempel di layar setelah selesai mengetik.")]
    public float displayDurationInSeconds = 5f;
    
    [Tooltip("Daftar hint untuk stage ini. Akan muncul berurutan, lalu berhenti selamanya jika sudah habis.")]
    [TextArea(2, 5)]
    public string[] stageHints = new string[]
    {
        "[Incoming Transmission]\nMake sure you check every room carefully. Sometimes the way out is not visible at first glance.",
        "[Incoming Transmission]\nIf you see a flashing panel, it might require an access code from a hidden clue.",
        "[Incoming Transmission]\nBe careful. There is an entity on this floor. Don't stand in the dark area for too long.",
        "[Incoming Transmission]\nRemember, you can't fight. Hiding is the only rational option."
    };

    [Header("Testing Tool")]
    [Tooltip("CENTANG INI SAAT TESTING: Membuat delayInMinutes dihitung sebagai DETIK agar cepat dites.")]
    public bool testModeInSeconds = false;

    [Header("Audio (Opsional)")]
    public AudioSource audioSource;
    public AudioClip beepSound;

    [Header("Stage Settings")]
    [Tooltip("Centang ini jika HintManager dipasang di Stage 1. HILANGKAN centang jika dipasang di Stage 2.")]
    public bool isStage1 = true;

    [Header("Typewriter Settings")]
    public float typeSpeed = 0.03f;

    // Memori sementara untuk Stage 2 (reset saat game ditutup)
    public static bool hasPlayedStage2Hints = false;

    private int currentHintIndex = 0;

    private void Start()
    {
        Debug.Log("HintManager: Start called. Memeriksa komponen...");

        // CEK APAKAH SUDAH PERNAH DIMAINKAN SEBELUMNYA (HANYA UNTUK STAGE 1)
        if (isStage1 && WelcomeStoryManager.hasPlayedStage1)
        {
            Debug.Log("HintManager: Stage 1 sudah pernah diselesaikan sebelumnya. Menghentikan HintManager secara permanen untuk sesi ini.");
            if (hintPanel != null) hintPanel.SetActive(false);
            return; // Gagalkan seluruh proses Start
        }

        // CEK APAKAH SUDAH PERNAH DIMAINKAN SEBELUMNYA (UNTUK STAGE 2)
        if (!isStage1 && hasPlayedStage2Hints)
        {
            Debug.Log("HintManager: Stage 2 sudah pernah dimasuki sebelumnya. Mematikan hint.");
            if (hintPanel != null) hintPanel.SetActive(false);
            return; 
        }
        
        // PAKSA PENGISIAN DARI KODE (Mengabaikan Inspector yang kosong)
        if (stageHints == null || stageHints.Length == 0 || string.IsNullOrWhiteSpace(stageHints[0]))
        {
            if (isStage1)
            {
                stageHints = new string[]
                {
                    "[Administrator]\nMake sure you check every room carefully. Sometimes the way out is not visible at first glance.",
                    "[-]\nIf you see a flashing panel, it might require an access code from a hidden clue.",
                    "[Vos]\nFour people, four important dates. Look for where they usually hide things. Those numbers are the key.",
                    "[Mirra]\nRemember, you can't fight. Hiding is the only rational option."
                };
            }
            else // JIKA INI ADALAH STAGE 2
            {
                stageHints = new string[]
                {
                    "[Unknown Transmission]\nThe vibration on this floor is not normal... There is a 'steel shadow' sleeping soundly in the hallway ahead.",
                    "[Unknown Transmission]\nThat machine has no heart, but it is programmed to detect your heartbeat. Step in silence..."
                };
            }
            Debug.Log("HintManager: Mengisi data teks secara otomatis berdasarkan pengaturan Stage!");
        }

        // Pastikan panel tersembunyi saat game dimulai
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }

        // Mulai sistem hint jika ada data hint
        if (stageHints.Length > 0)
        {
            Debug.Log("HintManager: stageHints memiliki " + stageHints.Length + " data. Memulai HintRoutine...");
            StartCoroutine(HintRoutine());
        }
        else
        {
            Debug.LogWarning("HintManager: stageHints kosong! Tidak ada hint yang bisa dijalankan.");
        }
    }

    private IEnumerator HintRoutine()
    {
        Debug.Log("HintManager: HintRoutine berjalan. Mencari WelcomeStoryManager...");
        
        // CARA PALING AMPUH: Cek langsung apakah Welcome Story sedang terbuka
        WelcomeStoryManager welcomeManager = FindObjectOfType<WelcomeStoryManager>();
        if (welcomeManager != null && welcomeManager.welcomePanel != null)
        {
            Debug.Log("HintManager: WelcomeStoryManager ditemukan. Menunggu panel Welcome tertutup...");
            // Selama panel welcome masih menyala, Hint akan diam menunggu tanpa peduli apa pun
            while (welcomeManager.welcomePanel.activeInHierarchy)
            {
                yield return null; // Tunggu per frame
            }
            Debug.Log("HintManager: Panel Welcome sudah tertutup! Melanjutkan proses...");
        }
        else
        {
            Debug.Log("HintManager: Tidak menemukan WelcomeStoryManager, lanjut saja...");
        }

        while (currentHintIndex < stageHints.Length)
        {
            // Tentukan delay (Menit vs Detik)
            float waitTime = testModeInSeconds ? delayInMinutes : (delayInMinutes * 60f);

            Debug.Log("HintManager: Menunggu selama " + waitTime + " detik sebelum memunculkan hint index " + currentHintIndex);
            
            // 1. Tunggu dalam keheningan selama waktu yang ditentukan
            yield return new WaitForSeconds(waitTime);

            Debug.Log("HintManager: Waktu tunggu selesai. Memunculkan Panel Hint...");

            // 2. Munculkan Panel
            if (hintPanel != null) hintPanel.SetActive(true);
            
            // 3. Mainkan Suara "Beep" (Pesan Masuk)
            if (audioSource != null && beepSound != null)
            {
                audioSource.PlayOneShot(beepSound);
            }

            // 4. Mulai efek mesin tik (Typewriter)
            if (hintText != null)
            {
                Debug.Log("HintManager: Mulai mengetik teks...");
                hintText.text = "";
                string currentText = stageHints[currentHintIndex];
                
                for (int i = 0; i <= currentText.Length; i++)
                {
                    hintText.text = currentText.Substring(0, i);
                    yield return new WaitForSeconds(typeSpeed);
                }
                Debug.Log("HintManager: Selesai mengetik teks. Menunggu durasi tampil (" + displayDurationInSeconds + " detik)...");
            }
            else
            {
                Debug.LogError("HintManager ERROR FATAL: Objek HintText belum dimasukkan ke dalam Inspector!");
            }

            // 5. Beri waktu pemain untuk membaca hint (Durasi Tampil)
            yield return new WaitForSeconds(displayDurationInSeconds);

            Debug.Log("HintManager: Durasi tampil selesai. Menyembunyikan hint...");

            // 6. Sembunyikan panel dan bersihkan teks
            if (hintPanel != null) hintPanel.SetActive(false);
            if (hintText != null) hintText.text = "";

            // Lanjut ke indeks hint berikutnya
            currentHintIndex++;
        }
        
        // Tandai bahwa hint di Stage 2 sudah pernah dimainkan agar tidak muncul lagi saat kembali
        if (!isStage1)
        {
            hasPlayedStage2Hints = true;
        }

        Debug.Log("HintManager: Semua hint di stage ini sudah ditampilkan.");
    }
}
