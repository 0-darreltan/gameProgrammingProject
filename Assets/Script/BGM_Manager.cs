using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public static BGM_Manager Instance; // Akses global

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Hancurkan duplikat
        }
    }
}