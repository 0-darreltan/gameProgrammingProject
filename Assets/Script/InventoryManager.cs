using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI; 
    private bool isPaused = false;

    void Start()
    {
        // Memastikan saat game mulai, inventory tertutup dan waktu jalan
        inventoryUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
        Time.timeScale = 0f; // Berhenti total
        isPaused = true;
    }

    public void ResumeGame()
    {
        inventoryUI.SetActive(false);
        Time.timeScale = 1f; // Jalan lagi
        isPaused = false;
    }
}