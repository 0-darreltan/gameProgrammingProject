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
        "When time stopped, only those who dared to move remained alive.",
        "The world's sky did not collapse, but froze along with all living creatures on the face of the earth.",
        "The ocean stopped in the middle of its waves. The wind died before reaching anywhere.",
        "Every single thing (people, animals, trees) froze in their last position, like a giant photo that would never move again.",
        "The cause of all this was in a tower named The Archive, a giant old tower towering in the middle of the city.",
        "At the top of the tower was kept The Heart of Time, an ancient machine that had kept the world running for thousands of years.",
        "When the machine stopped, time stopped too. No one knew who turned off the tower.",
        "No one knew the cause. And almost no one was left to ask.",
        "The mastermind behind the stopping of the ancient machine was Administrator. He wasn't a villain, but someone who had seen too many bad things and forgot how to see other perspectives.",
        "Sable wasn't a hero, just the only one who didn't freeze, driven by curiosity stronger than fear.",
        "Mirra wasn't a warrior, just a sister who refused to give up.",
        "The Archive still stood, holding more questions than answers, waiting for the next curious soul to enter."
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
