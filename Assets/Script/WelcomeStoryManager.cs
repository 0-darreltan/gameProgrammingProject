using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WelcomeStoryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject welcomePanel;
    public TextMeshProUGUI storyText;
    public Button nextButton;
    public Button prevButton; // Tombol untuk mundur

    [Header("Player Settings")]
    [Tooltip("Masukkan objek Karakter (Player) ke sini agar disembunyikan selama cutscene.")]
    public GameObject playerCharacter;

    [Header("Story Settings")]
    [TextArea(3, 10)]
    public string[] storySlides = new string[] 
    {
        // Slide 1
        "If you are reading this, it means you're in.\n\nGood.\n\nI don't know who you are,\nor why you are the one left\nwhen the others are not.",
        
        // Slide 2
        "This tower is no ordinary place. Every floor holds something, and perhaps the answers you didn't even know you were looking for.\n\nDon't rush. Observe first. This tower has been waiting for you...",
        
        // Slide 3
        "You will find other entities here.\n\nSome can be trusted.\nSome cannot.\nSome... I'm not even sure myself.\n\nTrust your instincts.\nRead every note you find.",
        
        // Slide 4
        "There is a way out of this tower. It depends on how far you are willing to look and how much you are willing to truly understand.\n\nYour choice.\nAlways your choice.",
        
        // Slide 5
        "I cannot tell you\nwhat you will find at the top.\n\nBut I can tell you this:\nThe world outside has stopped. You haven't!\n\nMaybe there is a reason for that."
    };
    
    public float typeSpeed = 0.05f;
    private int currentSlideIndex = 0;

    private void Start()
    {
        // Cari player secara otomatis jika kolom playerCharacter kosong
        if (playerCharacter == null)
        {
            playerCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        // Sembunyikan player saat cerita dimulai
        if (playerCharacter != null)
        {
            playerCharacter.SetActive(false);
        }

        if (welcomePanel != null)
        {
            welcomePanel.SetActive(true);
        }
        
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextSlide);
        }

        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(false);
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(PrevSlide);
        }

        if (storyText != null)
        {
            storyText.text = "";
        }

        Time.timeScale = 0f;
        currentSlideIndex = 0;

        if (storySlides.Length > 0)
        {
            StartCoroutine(TypeText(storySlides[currentSlideIndex]));
        }
        else
        {
            CloseWelcomeStory();
        }
    }

    private IEnumerator TypeText(string textToType)
    {
        storyText.text = "";

        for (int i = 0; i <= textToType.Length; i++)
        {
            storyText.text = textToType.Substring(0, i);
            yield return new WaitForSecondsRealtime(typeSpeed); 
        }

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(true);
        }

        // Tampilkan prevButton HANYA jika bukan di slide pertama
        if (prevButton != null)
        {
            if (currentSlideIndex > 0)
                prevButton.gameObject.SetActive(true);
            else
                prevButton.gameObject.SetActive(false);
        }
    }

    public void NextSlide()
    {
        currentSlideIndex++;

        if (currentSlideIndex < storySlides.Length)
        {
            // Sembunyikan kedua tombol saat mengetik
            if (nextButton != null) nextButton.gameObject.SetActive(false);
            if (prevButton != null) prevButton.gameObject.SetActive(false);
            
            StopAllCoroutines();
            StartCoroutine(TypeText(storySlides[currentSlideIndex]));
        }
        else
        {
            CloseWelcomeStory();
        }
    }

    public void PrevSlide()
    {
        if (currentSlideIndex > 0)
        {
            currentSlideIndex--;

            // Sembunyikan kedua tombol saat mengetik
            if (nextButton != null) nextButton.gameObject.SetActive(false);
            if (prevButton != null) prevButton.gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine(TypeText(storySlides[currentSlideIndex]));
        }
    }

    public void CloseWelcomeStory()
    {
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(false);
        }
        
        // Munculkan player kembali setelah cerita selesai
        if (playerCharacter != null)
        {
            playerCharacter.SetActive(true);
        }

        Time.timeScale = 1f;
    }
}
