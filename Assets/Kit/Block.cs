using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static blockMeneger;
using static Unity.Collections.AllocatorManager;
using UnityEditor;
using System.Reflection.Metadata;
using Unity.Mathematics;

public class Block : MonoBehaviour
{
    bool Isfly = true;
    public BlockType blockType;//เรียกค่าจากenumมาใช้เพราะสคลิปนี้ยังไม่รู้จักelementต่างๆ
    public blockMeneger spawner;
    public camera cam;
    Rigidbody2D rb;
    int speed = 2;
    int max = 3;
    int min = -1;
    int direction = 1;
    bool SpawnA = true;
    public float newScale;
    float newY;
    float newX;
    bool isMelt;
    bool spawMelt;
    bool spawMatoy;
    GameObject matoy;
    void melt()
    {
        newY = transform.localScale.y;
        newY -= Time.deltaTime;
        if (newY <= 0)
        {
            newY = 0;
            isMelt = false;
            Destroy(gameObject);
        }
       
        transform.localScale = new Vector2(transform.localScale.x, newY);

    }
    void melt2()
    {
        newX = transform.localScale.x;
        newX -= Time.deltaTime * 10;
        if (newX <= 0)
        {
            newX = 0;
            isMelt = false;
            Destroy(this.gameObject);
        }

        transform.localScale = new Vector2(newX, transform.localScale.y);

    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);

    }
    IEnumerator Wait2()
    {
        yield return new WaitForSeconds(4f);
       GameObject melt = Instantiate(spawner.items_melt, new UnityEngine.Vector2(transform.position.x, transform.position.y), UnityEngine.Quaternion.identity);

    }
    IEnumerator Wait_boom()
    {
        matoy = Instantiate(spawner.items_Matoy, new UnityEngine.Vector2(transform.position.x, cam.targetY), UnityEngine.Quaternion.identity);
        yield return new WaitForSeconds(8f);
        spawMatoy = false;


    }
    IEnumerator Waitboost()
    {
        speed *= 2;
       
        yield return new WaitForSeconds(5f);

        speed /= 2;
        //ส่วนใหญ่หน่วงเวลาจะต้องทำในเม็ดทอดเดียวกัน
    }
    IEnumerator Waitslow()
    {
        speed /= 2;

        yield return new WaitForSeconds(5f);

        speed *= 2;
        //ส่วนใหญ่หน่วงเวลาจะต้องทำในเม็ดทอดเดียวกัน
    }
    void Start()
    {
        cam = FindAnyObjectByType<camera>();
        spawner = FindAnyObjectByType<blockMeneger>();
        rb = GetComponent<Rigidbody2D>();
        BoxCollider2D col = GetComponent<BoxCollider2D>();
    }
    

    // Update is called once per frame
    //void set asset
    void speedController()
    {
        if (spawner.Score >= 33)
        {
            speed = 20;
        }
        else if (spawner.Score >= 25)
        {
            speed = 10;
        }
        else if (spawner.Score >= 17)
        {
            speed = 8;
        }
        else if (spawner.Score >= 13)
        {
            speed = 6;
        }
        else if (spawner.Score >= 7)
        {
            speed = 4;
        }
    }
    void FixedUpdate()
    {
        if (Isfly == true)
        {
            Controller();
        }
    }
    private void Update()
    {
        if(isMelt == true)
        {
            melt();
        }
        if (spawMatoy == true )
        {
            melt2();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale = 1;
            Isfly = false;
            StartCoroutine(Wait());
            rb.gravityScale = 9.81f;
        }
        speedController();
    }
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Block"))
        {
            Block otherBlock = collision2D.gameObject.GetComponent<Block>();
            if ((blockType == BlockType.Fire && otherBlock.blockType == BlockType.Fire))
            {
                StartCoroutine(Waitboost());
            }
            if ((blockType == BlockType.water && otherBlock.blockType == BlockType.water))
            {
                StartCoroutine(Waitslow());
            }
            if ((blockType == BlockType.Fire && otherBlock.blockType == BlockType.Plant)|| (blockType == BlockType.Plant && otherBlock.blockType == BlockType.Fire))
            {
                spawMelt = true;
                if (blockType == BlockType.Plant)
                {

                    isMelt = true;
                    if (spawMelt == true)
                    {
                       
                        spawMelt = false;
                    }
                    StartCoroutine(Wait2());
                    spawner.previousBlock = otherBlock.gameObject;
                }
                else if (otherBlock.blockType == BlockType.Plant)
                {
                    isMelt = true;

                    if (spawMelt == true)
                    {
                        spawMelt = false;
                    }
                    StartCoroutine(Wait2());
                    spawner.previousBlock = this.gameObject;
                }
                Debug.Log("Plant and Fire");
            }
            if ((blockType == BlockType.Fire && otherBlock.blockType == BlockType.water)
                || (blockType == BlockType.water && otherBlock.blockType == BlockType.Fire))
            {
                if (blockType == BlockType.Fire)
                {
                    if (spawMatoy)
                    {
                        return;
                    }
                    spawMatoy = true;
                    
                    //เสกหมอก
                    StartCoroutine((Wait_boom()));
                    spawner.previousBlock = otherBlock.gameObject;
                }
                else if (otherBlock.blockType == BlockType.Fire)
                {
                    if (spawMatoy)
                    {
                        return;
                    }
                    spawMatoy = true;

                    //เสกหมอก
                    StartCoroutine((Wait_boom()));
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
            }
        
            if ((blockType == BlockType.water && otherBlock.blockType == BlockType.Plant) || (blockType == BlockType.Plant && otherBlock.blockType == BlockType.water))
            {
                this.gameObject.transform.localScale = new Vector2(spawner.previousBlock.transform.localScale.x, spawner.newBlock.transform.localScale.y);
                spawner.newBlock.transform.localScale = this.gameObject.transform.localScale;
                Debug.Log("Plant and water");
            }
           /* if (SpawnA == true)
            {
                SpawnA = false;
                spawner.spawnBllock(newScale);
            } 
   
            else if (SpawnA == false )
            {
                spawner.spawnBllock(newScale);
            }*/
            spawner.Score  += 1;
            spawner.spawnBllock(newScale);
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
   
    /*public void CutBlock()
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

    }*/

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

