using UnityEngine;
public class SelfDestroy : MonoBehaviour {
    public float delay = 0.5f; // Sesuaikan dengan durasi animasi ledakan
    void Start() { Destroy(gameObject, delay); }
}