using System;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class blockMeneger : MonoBehaviour
{

   
    public GameObject[] items;
    public GameObject items_melt;
    public GameObject items_Matoy;
    public GameObject pawannapat;
    public Block Block;
    //enumตำแหน่งต้องตรงกับใน array
    public GameObject previousBlock;
    public GameObject newBlock;
    public GameObject oldblock;
    public GameObject title;
    public bool Iswater;
    public bool haspress = true;
    float spawnY;
    float height;
    float sum;
    public float newScale;
    public int blockCount = 1;
    public int matoy_count = 0;
    public int Score = 1;
    public TextMeshProUGUI scoreText;


    void Start()
    {
        UpdateScoreUI();
        spawnBllock();
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score;
        }
    }

    // Update is called once per frame
    void Update()
    { }
    public void spawnBllock()
    {
        /*        scaleX = UnityEngine.Random.Range(3, 8);
        */
        int xcaleY = 1;
        int itemDrop = UnityEngine.Random.Range(0, items.Length);
        /*        title.transform.localScale = new UnityEngine.Vector2(scaleX, xcaleY);
        */
       // float sum1 = pawannapat.transform.position.y + 1.7f;

        if (newBlock == null)
        {
            sum = pawannapat.transform.position.y + 15;
        }
        else
        {
                sum += 3.4f;
        }
        
       // pawannapat.transform.position = new UnityEngine.Vector2(pawannapat.transform.position.x, sum1);
        title = Instantiate(items[itemDrop], new UnityEngine.Vector2(-0.24f, 58), UnityEngine.Quaternion.identity);
        //สองบันทัดล่างคือเก็บค่าที่สุ่มได้ไปในสคลิป Block
        Block block = title.GetComponent<Block>();

        newBlock = title;
        block.spawner = this;
        if (previousBlock == null)
        {
            previousBlock = newBlock;
        }

        if (blockCount >= 1 )
        {
            blockCount ++;
        }
        UpdateScoreUI();
        //if else เป็นตัวช่วยกำหนด ใช้แต้ม
    }
}
