using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public BoxSpawner boxSpawner;
    
    public Box currentBox;
    public CameraFollow cameraFollow;
    public int score;
    public TextMeshProUGUI scoretxt;
    public int moveCameraCount;
    public int moveSpawnCount;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInput();
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentBox.DropBox();
        }
    }
    public void SpawnNewBox()
    {
        Invoke("NextBox", 1f);
    }
    public void NextBox()
    {
        boxSpawner.SpawnBox();
    }
    public void addScore()
    {
        score++;
        scoretxt.text = "" + score;
    }
    public void MoveCamera()
    {
        moveCameraCount++;
        if (moveCameraCount == 2)
        {
            moveCameraCount = 0;
            cameraFollow.targetPos.y += 1f;
        }
    }
    // public void MoveSpawner()
    // {
    //     moveSpawnCount++;
    //     if (moveSpawnCount == 2)
    //     {
    //         moveSpawnCount = 0;
    //         spawnerFollow.target.y += 1f;
    //     }
    // }
}
