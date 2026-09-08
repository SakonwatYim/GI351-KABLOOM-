using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าวัตถุที่เดินชนมี Tag ว่า "Player"
        if (other.CompareTag("Player"))
        {
            gameTimer.FinishGame();
        }
    }
}