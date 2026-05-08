using UnityEngine;
using System.Collections.Generic;
public class SpawnHandler : MonoBehaviour {
    [System.Serializable]
    public class TitikMuncul {
        public string idPintu;
        public Transform lokasi;
    }
    public List<TitikMuncul> daftarPintu = new List<TitikMuncul>();
    public GameObject player;
    void Start() {
        foreach (TitikMuncul tp in daftarPintu) {
            if (tp.idPintu == GlobalData.LastDoorID) {
                player.transform.position = tp.lokasi.position;
                return;
            }
        }
    }
}