using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEditor;

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
        //DontDestroyOnLoad(this.gameObject);
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

    public void OpenCredits()
    {
        creditsRef.SetActive(true);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    

    void Start()
    {
        // เช็คว่ามีค่าเซฟไว้หรือไม่ ถ้ามีให้โหลด ถ้าไม่มีให้ตั้งค่าเริ่มต้น
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            // ค่าเริ่มต้นของ Slider เมื่อเปิดเกมครั้งแรก (1 = 100%)
            musicSlider.value = 1f;
            sfxSlider.value = 1f;
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

    public void OpenHowToPlay()
    {
        howToPlayRef.SetActive(true);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void Back()
    {
        howToPlayRef.SetActive(false);
        creditsRef.SetActive(false);
        SettingRef.SetActive(false);
        
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    

    

    public void OpenSetting()
    {
        SettingRef.SetActive(true);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

     public void Exit()
    {
        SoundManager.GetInstance().PlaySound2D("Button");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
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
    float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
    float savedSfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

    // ถ้าค่าเซฟเก่าเป็น 0 หรือติดลบ ให้บังคับตั้งค่าเป็น 1 (เต็มหลอด 100%)
    if (savedMusic <= 0.001f) savedMusic = 1f;
    if (savedSfx <= 0.001f) savedSfx = 1f;

    musicSlider.value = savedMusic;
    sfxSlider.value = savedSfx;
    
    UpdateMusicVolume();
    UpdateSoundVolume();
    }
     
}
