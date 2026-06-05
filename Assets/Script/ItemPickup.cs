using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject gunInsidePod;    // Tarik 'gunsprite' (yang di dalam pod) ke sini
    public GameObject gunInInventory;  // Tarik 'GunIcon' (yang di UI) ke sini

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jika player menyentuh pod
        if (other.CompareTag("Player"))
        {
            // 1. Beritahu player sekarang sudah punya senjata
            other.GetComponent<PlayerMovement>().hasGun = true;

            // 2. Hilangkan gambar pistol yang ada di dalam Pod
            if (gunInsidePod != null) gunInsidePod.SetActive(false);

            // 3. Munculkan gambar pistol di UI Inventory (Equipped)
            if (gunInInventory != null) gunInInventory.SetActive(true);

            // 4. (Opsional) Hancurkan script ini agar tidak bisa diambil dua kali
            Destroy(this); 
            
            Debug.Log("Senjata Diambil! Sekarang kamu bisa menembak dengan tombol O");
        }
    }
}