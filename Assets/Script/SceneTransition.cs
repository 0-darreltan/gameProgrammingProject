using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour {
    public string sceneTujuan;
    public string idKeluar;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GlobalData.LastDoorID = idKeluar;
            SceneManager.LoadScene(sceneTujuan);
        }
    }
}