using UnityEngine;
using TMPro; // ใช้สำหรับ TextMeshPro
using UnityEngine.SceneManagement;

public class GameResultManager : MonoBehaviour
{
    public static GameResultManager Instance;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText; // ตัวแสดงเวลาตอนเล่น

    [Header("Result Popup UI")]
    public GameObject resultPopup;
    public TextMeshProUGUI resultTimeText;
    public TextMeshProUGUI bestTimeText;

    private float currentTime = 0f;
    private bool isTimerRunning = true;
    private bool isGameFinished = false;

    private void Awake()
    {
        // ทำ Singleton เพื่อให้สคริปต์อื่นเรียกใช้ได้ง่าย
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f; // รีเซ็ตเวลาเป็นปกติเมื่อเริ่มฉาก
        resultPopup.SetActive(false); // ซ่อน Popup
    }

    private void Update()
    {
        if (isTimerRunning && !isGameFinished)
        {
            currentTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(currentTime);
        }
    }

    public void GameFinished()
    {
        if (isGameFinished) return;

        isGameFinished = true;
        isTimerRunning = false;

        // หยุดเกม (หยุดฟิสิกส์และการเคลื่อนที่ของ Block)
        Time.timeScale = 0f;

        // คำนวณ Best Time (ดึงค่าเดิมที่เคยบันทึกไว้ ถ้าไม่มีให้ค่าเริ่มต้นเยอะๆ ไว้ก่อน)
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        // ถ้าเวลาปัจจุบันน้อยกว่า Best Time ให้บันทึกใหม่
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }

        // อัปเดตข้อความบน Popup
        //resultTimeText.text = "Time: " + FormatTime(currentTime);
        //bestTimeText.text = "Best Time: " + FormatTime(bestTime);

        // แสดง Popup
        resultPopup.SetActive(true);
    }

    // แปลงวินาทีเป็นรูปแบบ นาที:วินาที:มิลลิวินาที
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        int milliseconds = Mathf.FloorToInt((time - minutes * 60 - seconds) * 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // ฟังก์ชันสำหรับปุ่ม Play Again
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ฟังก์ชันสำหรับปุ่ม Main Menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        // เปลี่ยน "MainMenu" เป็นชื่อ Scene หน้าเมนูของคุณ
        SceneManager.LoadScene("MainMenu");
    }
}