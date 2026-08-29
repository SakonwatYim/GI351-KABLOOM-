using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class blockMeneger : MonoBehaviour
{

    public enum BlockType
    {
        Normal,
        Fire,
        Ice,
        Bomb
    }
    public GameObject[] items;
    public Block Block;
    //enumตำแหน่งต้องตรงกับใน array
    public GameObject previousBlock;
    public GameObject newBlock;
    public GameObject oldblock;
    public GameObject title;
   float spawnY = 2f;
   float height = 1f;
    public float newScale;
    int blockCount = 1;

    void Start()
    {
        int itemDrop = UnityEngine.Random.Range(0, items.Length);
        title = Instantiate(items[itemDrop], new UnityEngine.Vector2(2, 2), UnityEngine.Quaternion.identity);
        //สองบันทัดล่างคือเก็บค่าที่สุ่มได้ไปในสคลิป Block
        Block block = title.GetComponent<Block>();
        block.blockType = (BlockType)itemDrop;

        newBlock = title;
        block.spawner = this;
        if (previousBlock == null)
        {
            previousBlock = newBlock;
        }
    }

    // Update is called once per frame
    void Update()
    { }


    public void spawnBllock(float scale)
    {
        int itemDrop = UnityEngine.Random.Range(0, items.Length);
        title = Instantiate(items[itemDrop], new UnityEngine.Vector2(2, spawnY), UnityEngine.Quaternion.identity);
        spawnY += height;
        //สองบันทัดล่างคือเก็บค่าที่สุ่มได้ไปในสคลิป Block
        Block block = title.GetComponent<Block>();
        block.blockType = (BlockType)itemDrop;
        
        newBlock = title;
        block.spawner = this;
        if (previousBlock == null)
        {
           previousBlock = newBlock;
        }

       if (blockCount == 1 || blockCount == 2)
        {
            blockCount += 1;
        }
       if (blockCount > 2) 
        {
            title.transform.localScale = new UnityEngine.Vector2(scale, title.transform.localScale.y);
        }

        //if else เป็นตัวช่วยกำหนด ใช้แต้ม
    }
}

