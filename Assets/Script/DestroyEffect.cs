using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float delay = 0.5f; // Sesuaikan dengan durasi animasi ledakanmu

    void Start()
    {
        // Menghapus objek ini setelah waktu tertentu
        Destroy(gameObject, delay);
    }
}