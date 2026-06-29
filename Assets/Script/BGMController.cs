using UnityEngine;
using System.Collections;
using UnityEngine.Audio; // Tambahkan ini jika nanti ingin memakai Audio Mixer

public class BGMController : MonoBehaviour
{
    public static BGMController instance;

    [Header("Masukkan File Musik di Sini")]
    public AudioClip idleMusic;
    public AudioClip fightMusic;
    public AudioClip minibossMusic;

    [Header("Pengaturan Audio")]
    [Tooltip("Waktu transisi dalam detik")]
    public float fadeDuration = 1.5f; 
    [Tooltip("Kosongkan jika belum pakai Audio Mixer")]
    public AudioMixerGroup musicMixerGroup; 

    // Kita buat dua Audio Source tersembunyi
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private bool isPlayingSource1 = true;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Kita buat komponen Audio Source secara otomatis lewat kode 
        // agar Inspector tetap rapi
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        // Mengatur agar musik selalu berulang (loop)
        audioSource1.loop = true;
        audioSource2.loop = true;

        // Menyambungkan ke Audio Mixer (jika sudah ada)
        if (musicMixerGroup != null)
        {
            audioSource1.outputAudioMixerGroup = musicMixerGroup;
            audioSource2.outputAudioMixerGroup = musicMixerGroup;
        }
    }

    void Start()
    {
        // Set awal agar audioSource1 volumenya full, audioSource2 volumenya 0
        audioSource1.volume = 1f;
        audioSource2.volume = 0f;

        PlayMusic("Idle");
    }

    public void PlayMusic(string state)
    {
        AudioClip musicToPlay = null;

        switch (state)
        {
            case "Idle":
                musicToPlay = idleMusic;
                break;
            case "Fight":
                musicToPlay = fightMusic;
                break;
            case "Miniboss":
                musicToPlay = minibossMusic;
                break;
        }

        // Cari tahu Audio Source mana yang saat ini sedang aktif
        AudioSource activeSource = isPlayingSource1 ? audioSource1 : audioSource2;

        // Mencegah musik di-restart jika lagu yang diminta sama dengan yang sedang berputar
        if (activeSource.clip == musicToPlay) return;

        // Hentikan proses transisi sebelumnya (jika ada) dan mulai transisi baru
        StopAllCoroutines();
        StartCoroutine(CrossfadeMusic(musicToPlay));
    }

    private IEnumerator CrossfadeMusic(AudioClip nextClip)
    {
        // Tentukan mana yang lama (akan mengecil) dan mana yang baru (akan membesar)
        AudioSource activeSource = isPlayingSource1 ? audioSource1 : audioSource2;
        AudioSource newSource = isPlayingSource1 ? audioSource2 : audioSource1;

        // Siapkan lagu baru di Audio Source yang sedang tidak terpakai
        newSource.clip = nextClip;
        newSource.volume = 0f;
        newSource.Play();

        float timeElapsed = 0f;

        // Proses membesarkan dan mengecilkan volume secara bertahap
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / fadeDuration; // Hasilnya dari 0.0 sampai 1.0

            // Mathf.Lerp melakukan transisi angka secara mulus
            activeSource.volume = Mathf.Lerp(1f, 0f, progress); // 1 ke 0
            newSource.volume = Mathf.Lerp(0f, 1f, progress);    // 0 ke 1

            yield return null; // Tunggu ke frame berikutnya
        }

        // Pastikan volume akhir tepat
        activeSource.volume = 0f;
        newSource.volume = 1f;

        // Hentikan lagu yang lama sepenuhnya
        activeSource.Stop();

        // Tukar giliran Audio Source
        isPlayingSource1 = !isPlayingSource1;
    }
}