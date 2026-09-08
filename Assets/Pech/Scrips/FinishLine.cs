using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // ใช้เช็คกรณีตั้ง Collider เป็นแบบธรรมดา (ไม่ Trigger)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForBlock(collision.gameObject);
    }

    // ใช้เช็คกรณีตั้ง Collider เป็นแบบ Is Trigger 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        CheckForBlock(collider.gameObject);
    }

    private void CheckForBlock(GameObject obj)
    {
        // ตรวจสอบว่าสิ่งที่มาชนคือบล็อคหรือไม่ (อ้างอิงจากโค้ดเดิมที่ใช้ Layer "Block" หรือ Tag)
        if (obj.layer == LayerMask.NameToLayer("Block") || obj.GetComponent<Block>() != null)
        {
            // เรียกใช้ฟังก์ชัน GameFinished ใน GameResultManager
            if (GameResultManager.Instance != null)
            {
                GameResultManager.Instance.GameFinished();
            }
        }
    }
}