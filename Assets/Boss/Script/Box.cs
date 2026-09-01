using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    // base
    private float min_x = -2.32f, max_x = 2.32f;
    private bool canMove;
    private float move_speed = 2f;
    private Rigidbody2D rb;

    private bool ignoreCollision;
    private bool ignoreTrigger;
    private bool isGameOver;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }
    void Start()
    {
        canMove = true;
        if (Random.Range(0, 2) > 0)
        {
            move_speed *= -1f;
        }
        GameController.instance.currentBox = this;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBox();
    }

    void MoveBox()
    {
        if (canMove)
        {
            Vector3 temp = transform.position;
            temp.x += move_speed * Time.deltaTime;
            if (temp.x>max_x)
            {
                move_speed *= -1f;
            }else if (temp.x < min_x)
            {
                move_speed *= -1f;
            }
            transform.position = temp;
        }
    }
    public void DropBox()
    {
        canMove = false;
        rb.gravityScale = Random.Range(2,4);
    }
    private void OnCollisionEnter2D(Collision2D target)
    {
        if (ignoreCollision) return;
        if (target.gameObject.tag=="Ground")
        {
            ignoreCollision = true;
            GameController.instance.addScore();
            Invoke("OnGround", 1f);
        }
        if (target.gameObject.tag == "Box")
        {
            ignoreCollision = true;
            GameController.instance.addScore();
            GameController.instance.MoveCamera();
            Invoke("OnGround", 1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (ignoreTrigger) return;
        if (target.gameObject.tag=="GameOver")
        {
            CancelInvoke("OnGround");
            isGameOver = true;
            ignoreTrigger = true;
            Invoke("RestartGame", 2f);
        }
    }
    public void OnGround()
    {
        if (isGameOver) return;
        ignoreCollision = true;
        GameController.instance.SpawnNewBox();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
