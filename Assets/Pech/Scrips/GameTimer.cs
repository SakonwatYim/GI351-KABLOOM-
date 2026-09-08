using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("UI Text (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI timerText;     // ข้อความแสดงเวลาปัจจุบัน
    [SerializeField] private TextMeshProUGUI bestTimeText;  // ข้อความแสดงสถิติเวลาที่ดีที่สุด

    private float currentTime = 0f;
    private bool isTimerRunning = false;
    private bool hasStarted = false; // ตัวแปรล็อกไม่ให้กด Spacebar ซ้ำ
    private static float deltaTime;

    // คีย์สำหรับบันทึกลง PlayerPrefs
    private const string BEST_TIME_KEY = "Game_BestTime";

    void Start()
    {
        // โหลด High Score เดิมขึ้นมาโชว์ตอนเปิดเกม
        DisplayBestTime();
    }

    void Update()
    {
        // 1. กด Spacebar เพื่อเริ่มจับเวลา (ทำงานเฉพาะตอนที่ยังไม่เคยเริ่ม)
        if (Input.GetKeyDown(KeyCode.Space) && !hasStarted)
        {
            StartTimer();
        }

        // 2. ระหว่างที่เวลากำลังเดิน
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            timerText.text = FormatTime(currentTime);
        }
    }

    public void StartTimer()
    {
        hasStarted = true;
        isTimerRunning = true;
        currentTime = 0f;
    }

    // 3. ฟังก์ชันเรียกตอนจบเกม (เช่น ชนเส้นชัย หรือทำภารกิจเสร็จ)
    public void FinishGame()
    {
        // ถ้าเวลายังไม่เริ่มหรือหยุดไปแล้ว ไม่ต้องทำอะไร
        if (!isTimerRunning) return;

        isTimerRunning = false;
        CheckAndUpdateBestTime(currentTime);
    }

    private void CheckAndUpdateBestTime(float finalTime)
    {
        // ดึงสถิติเดิม ถ้าไม่เคยมี ให้ตั้งเป็นค่าอนันต์ (Infinity) ไว้ก่อน
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, Mathf.Infinity);

        // ถ้าเวลาที่ทำได้ "น้อยกว่า" สถิติเดิม = สถิติใหม่ (เร็วขึ้น)
        if (finalTime < bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, finalTime);
            PlayerPrefs.Save();
            Debug.Log($"ทำลายสถิติใหม่! เวลา: {FormatTime(finalTime)}");
        }

        DisplayBestTime();
    }

    private void DisplayBestTime()
    {
        if (PlayerPrefs.HasKey(BEST_TIME_KEY))
        {
            float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY);
            bestTimeText.text = "Best: " + FormatTime(bestTime);
        }
        else
        {
            bestTimeText.text = "Best: --:--.--";
        }
    }

    // ฟังก์ชันแปลงตัวเลขวินาทีเป็นรูปแบบ นาที:วินาที.มิลลิวินาที (00:00.00)
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    // ฟังก์ชันรีเซ็ตรอบใหม่ (เรียกใช้เมื่อกดปุ่ม Restart)
    public void ResetGame()
    {
        hasStarted = false;
        isTimerRunning = false;
        currentTime = 0f;
        timerText.text = "00:00.00";
    }

    // ตัวเลือกเสริม: ล้างสถิติ High score
    public void ClearBestTime()
    {
        PlayerPrefs.DeleteKey(BEST_TIME_KEY);
        DisplayBestTime();
    }
}
