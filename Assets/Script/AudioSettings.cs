using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

    [Header("UI Slider")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("UI Toggle")]
    public Toggle masterMuteToggle;
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;

    // Variabel ini menampung nilai sementara sebelum tombol Apply ditekan
    private float tempMaster, tempMusic, tempSFX;

    void Start()
    {
        // Load nilai awal ke slider
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterMuteToggle.isOn = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuteToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        // Terapkan langsung ke mixer
        ApplySettings();
    }

    // Fungsi yang dipanggil oleh tombol APPLY
    public void ApplySettings()
    {
        Debug.Log("Menerapkan Pengaturan Audio...");
        // Simpan ke PlayerPrefs
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        // Terapkan ke Mixer
        UpdateVolumes();
    }

    // Fungsi pembantu untuk update mixer
    private void UpdateVolumes()
    {
        mainMixer.SetFloat("MasterVolume", masterMuteToggle.isOn ? -80f : Mathf.Log10(masterSlider.value) * 20);
        mainMixer.SetFloat("MusicVolume", musicMuteToggle.isOn ? -80f : Mathf.Log10(musicSlider.value) * 20);
        mainMixer.SetFloat("SFXVolume", sfxMuteToggle.isOn ? -80f : Mathf.Log10(sfxSlider.value) * 20);
    }

    // Fungsi Toggle Mute (Tetap Real-time)
    public void ToggleMasterMute() { PlayerPrefs.SetInt("MasterMuted", masterMuteToggle.isOn ? 1 : 0); UpdateVolumes(); }
    public void ToggleMusicMute() { PlayerPrefs.SetInt("MusicMuted", musicMuteToggle.isOn ? 1 : 0); UpdateVolumes(); }
    public void ToggleSFXMute() { PlayerPrefs.SetInt("SFXMuted", sfxMuteToggle.isOn ? 1 : 0); UpdateVolumes(); }
}