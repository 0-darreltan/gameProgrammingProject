using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IntroCutscene : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cutscenePanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;

    [Header("Cutscene Settings")]
    public string nextSceneName = "Tutorial1";
    public float typeSpeed = 0.08f;
    public float paragraphPause = 2f;
    public bool autoCreateFallbackUI = true;

    [TextArea(3, 10)]
    public string[] paragraphs = new string[]
    {
        "THE ARCHIVE",
        "Ketika waktu berhenti, hanya mereka yang berani bergerak yang masih hidup.",
        "Langit dunia tidak runtuh melainkan membeku dengan segala makhluk yang ada di muka bumi.",
        "Laut berhenti di tengah ombaknya. Angin mati sebelum sampai ke mana pun.",
        "Seluruh isi dunia (manusia, hewan, pohon) berhenti dalam posisi terakhir mereka, seperti foto raksasa yang tidak pernah bergerak lagi.",
        "Penyebab dari semua ini berada di menara bernama The Archive yang merupakan menara tua raksasa yang menjulang di tengah kota.",
        "Di puncak menara tersimpan The Heart of Time yaitu mesin kuno yang selama ribuan tahun menjaga dunia tetap berjalan.",
        "Ketika mesin itu berhenti, waktu pun ikut berhenti. Tidak ada yang tahu siapa yang menonaktifkan menara tersebut.",
        "Tidak ada yang tahu penyebabnya. Dan hampir tidak ada yang tersisa untuk bertanya.",
        "Dalang dibalik berhentinya mesin kuno ialah Administrator. Ia bukan penjahat namun orang yang melihat terlalu banyak hal buruk sampai lupa cara melihat sudut pandang lain.",
        "Sable bukan pahlawan, ia hanya satu-satunya yang tidak membeku dengan rasa ingin tahu yang lebih besar dari rasa takutnya.",
        "Mirra bukanlah pejuang, ia hanya kakak yang tidak mau menyerah.",
        "The Archive masih berdiri yang menyimpan lebih banyak pertanyaan dari jawaban, menunggu siapapun berikutnya yang cukup penasaran untuk masuk."
    };

    private void Awake()
    {
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(false);
        }
    }

    private void Start()
    {
        PlayIntroCutscene();
    }

    private void EnsureUI()
    {
        if (cutscenePanel != null && titleText != null && contentText != null)
        {
            return;
        }

        if (autoCreateFallbackUI)
        {
            CreateFallbackUI();
        }
    }

    public void PlayIntroCutscene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("IntroCutscene: nextSceneName belum diisi.");
            return;
        }

        EnsureUI();

        if (cutscenePanel == null || titleText == null || contentText == null)
        {
            Debug.LogWarning("IntroCutscene: Referensi UI tidak lengkap. Memuat scene langsung.");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        cutscenePanel.SetActive(true);
        StartCoroutine(RunCutscene());
    }

    public void SkipCutscene()
    {
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(false);
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator RunCutscene()
    {
        titleText.text = paragraphs.Length > 0 ? paragraphs[0] : "";
        contentText.text = "";

        for (int i = 1; i < paragraphs.Length; i++)
        {
            yield return TypeText(paragraphs[i]);
            yield return new WaitForSeconds(paragraphPause);
            contentText.text = "";
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator TypeText(string fullText)
    {
        contentText.text = "";

        for (int i = 0; i <= fullText.Length; i++)
        {
            contentText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private void CreateFallbackUI()
    {
        Debug.Log("IntroCutscene: Membuat UI fallback karena referensi belum diisi.");

        var canvasObject = new GameObject("IntroCutsceneCanvas");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        var panelObject = new GameObject("CutscenePanel");
        panelObject.transform.SetParent(canvasObject.transform, false);
        var panelImage = panelObject.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.85f);

        var panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 0f);
        panelRect.anchorMax = new Vector2(1f, 1f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var titleObject = new GameObject("CutsceneTitle");
        titleObject.transform.SetParent(panelObject.transform, false);
        titleText = titleObject.AddComponent<TextMeshProUGUI>();
        titleText.text = "";
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontSize = 48f;
        titleText.color = Color.white;
        titleText.enableWordWrapping = true;

        var titleRect = titleObject.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.1f, 0.7f);
        titleRect.anchorMax = new Vector2(0.9f, 0.92f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        var contentObject = new GameObject("CutsceneContent");
        contentObject.transform.SetParent(panelObject.transform, false);
        contentText = contentObject.AddComponent<TextMeshProUGUI>();
        contentText.text = "";
        contentText.alignment = TextAlignmentOptions.TopLeft;
        contentText.fontSize = 28f;
        contentText.color = Color.white;
        contentText.enableWordWrapping = true;

        var contentRect = contentObject.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.08f, 0.08f);
        contentRect.anchorMax = new Vector2(0.92f, 0.68f);
        contentRect.offsetMin = Vector2.zero;
        contentRect.offsetMax = Vector2.zero;

        cutscenePanel = panelObject;
        cutscenePanel.SetActive(false);
    }
}
