using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    [Header("Ui ref")]
    public GameObject howToPlayRef;

    private void Start()
    {
        if (howToPlayRef != null)
        {
            howToPlayRef.SetActive(false);
        }
    }

    public void HowToPlay()
    {
        howToPlayRef.SetActive(true);
    }

    public void Back()
    {
        howToPlayRef.SetActive(false);
    }
}
