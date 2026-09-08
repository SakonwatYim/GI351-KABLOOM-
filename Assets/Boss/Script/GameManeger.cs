// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// /// <summary>
// /// ตัวควบคุมเกมหลัก: สปอว์นบล็อคใหม่, ตรวจการกดเว้นวรรคเพื่อวาง,
// /// คำนวณส่วนที่ทับกัน (ตัดส่วนเกินทิ้ง), นับคะแนน และบันทึก High Score
// /// วิธีใช้: สร้าง Empty GameObject ชื่อ GameManager แล้วแปะสคริปต์นี้
// /// </summary>
// public class GameManeger : MonoBehaviour
// {
//     public static GameManeger Instance;

//     [Header("Prefab & Setup")]
//     public GameObject blockPrefab;      // Prefab บล็อค (มี SpriteRenderer)
//     public Transform startBlock;        // บล็อคฐานที่วางไว้ในฉากตั้งแต่แรก

//     [Header("Block Settings")]
//     public float blockHeight = 0.5f;
//     public float startSpeed = 3f;
//     public float speedIncrease = 0.15f;
//     public float maxSpeed = 10f;
//     public float moveRange = 3f;        // ระยะซ้าย-ขวาที่บล็อคเคลื่อนที่ได้

//     [Header("Camera")]
//     public Transform cameraTransform;
//     public float cameraYOffset = 3f;

//     [Header("UI")]
//     public Text scoreText;
//     public Text highScoreText;
//     public GameObject gameOverPanel;

//     private Transform previousBlock;
//     private GameObject currentBlockObj;
//     private Box currentMover;

//     private float currentSpeed;
//     private int score = 0;
//     private int highScore = 0;
//     private bool isGameOver = false;

//     private const string HighScoreKey = "StackGame_HighScore";

//     void Awake()
//     {
//         Instance = this;
//     }

//     void Start()
//     {
//         highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
//         UpdateUI();

//         if (gameOverPanel != null) gameOverPanel.SetActive(false);

//         previousBlock = startBlock;
//         currentSpeed = startSpeed;

//         SpawnNewBlock();
//     }

//     void Update()
//     {
//         if (isGameOver)
//         {
//             if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
//             {
//                 Restart();
//             }
//             return;
//         }

//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             DropBlock();
//         }
//     }

//     void SpawnNewBlock()
//     {
//         Vector3 prevScale = previousBlock.localScale;
//         float newY = previousBlock.position.y + blockHeight;

//         // สุ่มเริ่มจากด้านซ้ายหรือขวาสลับกันไป
//         float startX = (Random.value > 0.5f) ? -moveRange : moveRange;

//         currentBlockObj = Instantiate(blockPrefab, new Vector3(startX, newY, 0f), Quaternion.identity);
//         currentBlockObj.transform.localScale = new Vector3(prevScale.x, blockHeight, 1f);

//         currentMover = currentBlockObj.GetComponent<Box>();
//         if (currentMover == null) currentMover = currentBlockObj.AddComponent<Box>();

//         currentMover.speed = currentSpeed;
//         currentMover.leftBound = -moveRange;
//         currentMover.rightBound = moveRange;
//         currentMover.isMoving = true;

//         MoveCamera(newY);
//     }

//     void DropBlock()
//     {
//         currentMover.StopMoving();

//         float prevX = previousBlock.position.x;
//         float prevWidth = previousBlock.localScale.x;
//         float curX = currentBlockObj.transform.position.x;
//         float curWidth = currentBlockObj.transform.localScale.x;

//         float prevLeft = prevX - prevWidth / 2f;
//         float prevRight = prevX + prevWidth / 2f;
//         float curLeft = curX - curWidth / 2f;
//         float curRight = curX + curWidth / 2f;

//         float overlapLeft = Mathf.Max(prevLeft, curLeft);
//         float overlapRight = Mathf.Min(prevRight, curRight);
//         float overlapWidth = overlapRight - overlapLeft;

//         if (overlapWidth <= 0.02f)
//         {
//             // ต่อไม่โดนเลย -> จบเกม
//             EndGame();
//             return;
//         }

//         // สร้างชิ้นส่วนที่ถูกตัดทิ้งให้ตกลงมา (แค่ effect ให้เห็นภาพ)
//         float leftoverWidthLeft = curLeft < overlapLeft ? overlapLeft - curLeft : 0f;
//         float leftoverWidthRight = curRight > overlapRight ? curRight - overlapRight : 0f;

//         if (leftoverWidthLeft > 0.01f)
//         {
//             CreateFallingPiece(curLeft, overlapLeft, currentBlockObj.transform.position.y, currentBlockObj.transform.localScale.y);
//         }
//         if (leftoverWidthRight > 0.01f)
//         {
//             CreateFallingPiece(overlapRight, curRight, currentBlockObj.transform.position.y, currentBlockObj.transform.localScale.y);
//         }

//         // ปรับบล็อคปัจจุบันให้เหลือแค่ส่วนที่ทับกับบล็อคก่อนหน้า
//         float overlapCenter = (overlapLeft + overlapRight) / 2f;
//         currentBlockObj.transform.position = new Vector3(overlapCenter, currentBlockObj.transform.position.y, 0f);
//         currentBlockObj.transform.localScale = new Vector3(overlapWidth, currentBlockObj.transform.localScale.y, 1f);

//         previousBlock = currentBlockObj.transform;

//         score++;
//         UpdateUI();

//         currentSpeed = Mathf.Min(currentSpeed + speedIncrease, maxSpeed);

//         SpawnNewBlock();
//     }

//     void CreateFallingPiece(float leftX, float rightX, float y, float height)
//     {
//         float width = rightX - leftX;
//         if (width <= 0f) return;

//         GameObject piece = Instantiate(blockPrefab, new Vector3((leftX + rightX) / 2f, y, 0f), Quaternion.identity);
//         piece.transform.localScale = new Vector3(width, height, 1f);

//         Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
//         rb.gravityScale = 3f;

//         if (piece.GetComponent<Collider2D>() == null)
//         {
//             piece.AddComponent<BoxCollider2D>();
//         }

//         Destroy(piece, 2f);
//     }

//     void MoveCamera(float targetY)
//     {
//         if (cameraTransform == null) return;
//         Vector3 pos = cameraTransform.position;
//         cameraTransform.position = new Vector3(pos.x, targetY + cameraYOffset, pos.z);
//     }

//     void EndGame()
//     {
//         isGameOver = true;

//         if (currentMover != null) currentMover.StopMoving();

//         if (score > highScore)
//         {
//             highScore = score;
//             PlayerPrefs.SetInt(HighScoreKey, highScore);
//             PlayerPrefs.Save();
//         }

//         UpdateUI();

//         if (gameOverPanel != null) gameOverPanel.SetActive(true);
//     }

//     void UpdateUI()
//     {
//         if (scoreText != null) scoreText.text = "Score: " + score;
//         if (highScoreText != null) highScoreText.text = "High Score: " + highScore;
//     }

//     public void Restart()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }
// }