using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public AudioMixer audiomixer;
    public Slider musicSlider;
    public Slider sfxSlider;


    [Header("Ui ref")]
    public GameObject howToPlayRef;
    public GameObject creditsRef;
    public GameObject SettingRef;
    public void Play()
    {
        SceneManager.LoadSceneAsync("kit");
        SoundManager.GetInstance().PlaySound2D("Button");
        MusicManager.GetInstance().PlayMusic("Gameplay");
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync("Credits");
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    

    void Start()
    {
        LoadSetting();
        if (howToPlayRef != null)
        {
            howToPlayRef.SetActive(false);
        }
        MusicManager.GetInstance().PlayMusic("MainMenu");
    }

    public void HowToPlay()
    {
        howToPlayRef.SetActive(true);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void Back()
    {
        howToPlayRef.SetActive(false);
        // creditsRef.SetActive(false);
        SettingRef.SetActive(false);
        
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void OpenSetting()
    {
        SettingRef.SetActive(true);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void Quit()
    {
        SoundManager.GetInstance().PlaySound2D("Button");
        Application.Quit();
        
    }

    public void UpdateMusicVolume(float volume)
    {
        audiomixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void UpdateSFXVolume(float volume)
    {
        audiomixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void SaveSetting()
    {
        audiomixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        audiomixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        
    }

    public void LoadSetting()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        
    }
}
