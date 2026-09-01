using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static blockMeneger;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{
    bool Isfly = true;
    public BlockType blockType;//เรียกค่าจากenumมาใช้เพราะสคลิปนี้ยังไม่รู้จักelementต่างๆ
    public blockMeneger spawner;
    Rigidbody2D rb;
    int speed = 2;
    int max = 3;
    int min = -1;
    int direction = 1;
    bool SpawnA = true;
    public float newScale;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(20f, col.size.y);
    }

    // Update is called once per frame
   
    void FixedUpdate()
    {
        if (Isfly == true)
        {
            Controller();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale = 1;
            Isfly = false;

        }
       //Debug.Log($"{spawner.Score}");
    }
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Block"))
        {
            Block otherBlock = collision2D.gameObject.GetComponent<Block>();
            if ((blockType == BlockType.Fire && otherBlock.blockType == BlockType.Plant)|| (blockType == BlockType.Plant && otherBlock.blockType == BlockType.Fire))
            {
                if (blockType == BlockType.Plant)
                {
                    Destroy(this.gameObject);
                    spawner.previousBlock = otherBlock.gameObject;
                }
                else if (otherBlock.blockType == BlockType.Plant)
                {
                    Destroy(otherBlock.gameObject);
                    spawner.previousBlock = this.gameObject;
                }

                Debug.Log("Plant and Fire");
            }
            if ((blockType == BlockType.Fire && otherBlock.blockType == BlockType.water)
                || (blockType == BlockType.water && otherBlock.blockType == BlockType.Fire))
            {
                if (blockType == BlockType.Fire)
                {
                    Destroy(this.gameObject);
                    spawner.previousBlock = otherBlock.gameObject;
                }
                else if (otherBlock.blockType == BlockType.Fire)
                {
                    Destroy(otherBlock.gameObject);
                    spawner.previousBlock = this.gameObject;

                }
                Debug.Log("Fire and water");
            }

            if (gameObject != spawner.newBlock)

            {
                return;
            //เช็คให้บล็อกล่าสุดคือ new block 
            }
            
            if (spawner.previousBlock != null)
            {
                //ถ้าบล็อกล่าสุดมีอยู่แล้วให้ทำงานในฟังก์ชั่น
                spawner.previousBlock = collision2D.gameObject;//บล็อกก่อนหน้าคือบล็ํอกที่ชน
                spawner.newBlock = gameObject;
                CutBlock();
            }
            if (SpawnA == true)
            {
                SpawnA = false;
                spawner.spawnBllock(newScale);
            }
            if ((blockType == BlockType.water && otherBlock.blockType == BlockType.Plant) || (blockType == BlockType.Plant && otherBlock.blockType == BlockType.water))
            {
                this.gameObject.transform.localScale = new Vector2(spawner.previousBlock.transform.localScale.x, spawner.newBlock.transform.localScale.y);
                spawner.newBlock.transform.localScale = this.gameObject.transform.localScale;
                Debug.Log("Plant and water");
            }


        }
        
        if (collision2D.gameObject.CompareTag("floor"))
        {
           if (SpawnA == true)
            {
            spawner.spawnBllock(7);
            }
            SpawnA = false;
            spawner.Score += 1;

        }
    }
    public void GameOver()
    {
        

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//สั่งโหลดSceneเดิม

    }
    public void CutBlock()
    {
        float oldX = spawner.previousBlock.transform.position.x;
        float oldScale = spawner.previousBlock.transform.localScale.x;

        float newX = spawner.newBlock.transform.position.x;
        newScale = spawner.newBlock.transform.localScale.x;

        float oldL = oldX - oldScale / 2f;
        float oldR = oldX + oldScale / 2f;

        float newL = newX - newScale / 2f;
        float newR = newX + newScale / 2f;

        // ไม่ทับกัน
        if (newR < oldL || newL > oldR)
        {
            Debug.Log("GAME OVER CALLED");
            GameOver();
        
        }

        // เกินทางซ้าย
        if (newL < oldL)
        {
            newScale = newR - oldL;
            newX = (oldL + newR) / 2f;
        }
        // เกินทางขวา
        else if (newR > oldR)
        {
            newScale = oldR - newL;
            newX = (newL + oldR) / 2f;
        }

        spawner.newBlock.transform.position = new Vector2(
            newX,
            spawner.newBlock.transform.position.y
        );

            spawner.newBlock.transform.localScale = new Vector2(
            newScale,
            spawner.newBlock.transform.localScale.y

        );
        spawner.newScale = newScale;
        spawner.Score += 1;

    }

    public void Controller()
    {
        
        //ใช้rb.moveposition
        Vector2 new_position = rb.position + Vector2.right* direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(new_position); 
        if (rb.position.x >= max) 
        {
            direction = -1;
        }
        else if (rb.position.x <= min)
        {
            direction = 1;
        }


    }

   // public void Star()
    
     //รอเรื่องคะแนน
}

