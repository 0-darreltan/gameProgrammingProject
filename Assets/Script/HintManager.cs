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
        "[Transmisi Masuk]\nPastikan kamu memeriksa setiap ruangan dengan teliti. Terkadang jalan keluar tidak terlihat di pandangan pertama.",
        "[Transmisi Masuk]\nJika kamu melihat panel berkedip, itu mungkin membutuhkan kode akses dari sebuah petunjuk tersembunyi.",
        "[Transmisi Masuk]\nHati-hati. Ada entitas di lantai ini. Jangan terlalu lama berdiri di area yang gelap.",
        "[Transmisi Masuk]\nIngat, kamu tidak bisa bertarung. Bersembunyi adalah satu-satunya pilihan rasional."
    };

    [Header("Testing Tool")]
    [Tooltip("CENTANG INI SAAT TESTING: Membuat delayInMinutes dihitung sebagai DETIK agar cepat dites.")]
    public bool testModeInSeconds = false;

    [Header("Audio (Opsional)")]
    public AudioSource audioSource;
    public AudioClip beepSound;

    [Header("Typewriter Settings")]
    public float typeSpeed = 0.03f;

    private int currentHintIndex = 0;

    private void Start()
    {
        Debug.Log("HintManager: Start called. Memeriksa komponen...");
        
        // PAKSA PENGISIAN DARI KODE (Mengabaikan Inspector yang kosong)
        if (stageHints == null || stageHints.Length == 0 || string.IsNullOrWhiteSpace(stageHints[0]))
        {
            stageHints = new string[]
            {
                "[Administrator]\nPastikan kamu memeriksa setiap ruangan dengan teliti. Terkadang jalan keluar tidak terlihat di pandangan pertama.",
                "[-]\nJika kamu melihat panel berkedip, itu mungkin membutuhkan kode akses dari sebuah petunjuk tersembunyi.",
                "[Vos]\nEmpat orang, empat tanggal penting. Cari di tempat mereka biasa menyembunyikan sesuatu. Angka-angka itu adalah kuncinya.",
                "[Mirra]\nIngat, kamu tidak bisa bertarung. Bersembunyi adalah satu-satunya pilihan rasional."
            };
            Debug.Log("HintManager: Mengisi data teks secara otomatis dari dalam kode karena di Inspector kosong!");
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
        
        Debug.Log("HintManager: Semua hint di stage ini sudah ditampilkan.");
    }
}
