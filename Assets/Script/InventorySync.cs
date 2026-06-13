using UnityEngine;

public class InventorySync : MonoBehaviour
{
    public GameObject gunIcon; // Tarik GunIcon (UI) ke sini
    public PlayerMovement playerScript; // Tarik Player ke sini

    void Start()
    {
        // 1. Cek Memori: Apakah kita sudah punya pistol?
        if (GlobalData.punyaPistol)
        {
            // Nyalakan ikon di UI
            if (gunIcon != null) gunIcon.SetActive(true);

            // Beritahu Player di map ini bahwa dia bisa menembak
            if (playerScript != null) playerScript.hasGun = true;
        }
        else
        {
            // Jika belum punya, pastikan mati
            if (gunIcon != null) gunIcon.SetActive(false);
            if (playerScript != null) playerScript.hasGun = false;
        }
    }
}