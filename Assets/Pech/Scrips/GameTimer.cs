using UnityEngine;
using TMPro; // สำหรับใช้ UI TextMeshPro

public class GameTimer : MonoBehaviour
{
    [Header("Game Settings")]
    public float targetHeight = 20f; // กำหนดความสูง Y ที่ต้องการให้จบเกม

    [Header("UI References")]
    public TextMeshProUGUI timerText;      // UI แสดงเวลาปัจจุบัน
    public TextMeshProUGUI bestTimeText;   // UI แสดงเวลาที่ดีที่สุด
    public GameObject winPanel;            // หน้าต่าง UI ตอนชนะ

    private blockMeneger spawner;
    private float currentTime = 0f;
    private bool isGameOver = false;

    void Start()
    {
        // ค้นหาสคริปต์ blockMeneger ในฉากอัตโนมัติ
        spawner = FindObjectOfType<blockMeneger>();

        // ปิดหน้าต่างชนะตอนเริ่มเกม
        if (winPanel != null) winPanel.SetActive(false);

        // ดึงค่าเวลาที่ดีที่สุดมาแสดงตอนเริ่ม (ถ้ายังไม่มีจะเป็นเลขเยอะมาก)
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        if (bestTime < float.MaxValue)
        {
            bestTimeText.text = "Best Time: " + FormatTime(bestTime);
        }
        else
        {
            bestTimeText.text = "Best Time: --:--";
        }
    }

    void Update()
    {
        if (isGameOver || spawner == null) return;

        // นับเวลาเดินหน้า
        currentTime += Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(currentTime);
        }

        // ตรวจสอบว่าความสูงของ pawannapat ถึงจุดที่กำหนดหรือยัง
        if (spawner.pawannapat != null && spawner.pawannapat.transform.position.y >= targetHeight)
        {
            TriggerWinCondition();
        }
    }

    void TriggerWinCondition()
    {
        isGameOver = true;

        // หยุดเวลาในเกม (ทำให้บล็อกไม่ขยับและไม่ร่วง โดยไม่ต้องไปแก้สคริปต์ Block)
        Time.timeScale = 0f;

        // เปิดหน้า UI ชนะ
        if (winPanel != null) winPanel.SetActive(true);

        // ตรวจสอบและบันทึกสถิติใหม่
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        if (currentTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", currentTime);
            PlayerPrefs.Save(); // บันทึกข้อมูลลงเครื่อง

            bestTimeText.text = "NEW BEST TIME!\n" + FormatTime(currentTime);
        }
        else
        {
            bestTimeText.text = "Time: " + FormatTime(currentTime) + "\nBest: " + FormatTime(bestTime);
        }
    }

    // แปลงวินาทีให้เป็นรูปแบบ นาที:วินาที:มิลลิวินาที
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}