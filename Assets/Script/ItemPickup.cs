using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject gunInsidePod;
    public GameObject gunInInventory;

    void Start() {
        // Jika di memori tercatat sudah punya pistol, hilangkan pod-nya saat map loading
        if (GlobalData.punyaPistol) {
            if (gunInsidePod != null) gunInsidePod.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tulis ke Memori Global
            GlobalData.punyaPistol = true;

            // Beritahu Player
            other.GetComponent<PlayerMovement>().hasGun = true;

            if (gunInsidePod != null) gunInsidePod.SetActive(false);
            if (gunInInventory != null) gunInInventory.SetActive(true);
            
            Destroy(this);
        }
    }
}