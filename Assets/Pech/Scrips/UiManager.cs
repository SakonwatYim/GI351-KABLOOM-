using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    private static UiManager instance;
    public static UiManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    [Header("Ui ref")]
    public GameObject howToPlayRef;
    public GameObject creditsRef;
    public GameObject SettingRef;
    public void Play()
    {
        SceneManager.LoadSceneAsync("kit");
        SoundManager.GetInstance().PlaySound2D("ClickPlay");
        MusicManager.GetInstance().PlayMusic("Gameplay");
        
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync("Credits");
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    

    void Start()
    {
        LoadVolume();
        MusicManager.GetInstance().PlayMusic("MainMenu");
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            UpdateMusicVolume();
            UpdateSoundVolume();
        }
        
        if (howToPlayRef != null)
        {
            howToPlayRef.SetActive(false);
        }
        if (creditsRef != null)
        {
            creditsRef.SetActive(false);
        }
        if (SettingRef != null)
        {
            SettingRef.SetActive(false);
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

      public void UpdateMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SaveSetting()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        
        UpdateMusicVolume();
        UpdateSoundVolume();
    }
     public void buttonClickSound()
    {
        SoundManager.GetInstance().PlaySound2D("ClickPlay");
    }
}
