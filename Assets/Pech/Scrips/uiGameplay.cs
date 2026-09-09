using UnityEngine;
using UnityEngine.SceneManagement;

public class uiGameplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Retry()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SoundManager.GetInstance().PlaySound2D("Button");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        SoundManager.GetInstance().PlaySound2D("Button");
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
