using UnityEngine;

public class camera : MonoBehaviour
{

    public blockMeneger spawner;

    public float speed = 5f;
    public float targetY;

    void LateUpdate()
    {
        
        /* if (spawner.newBlock != null)
        {
            targetY = spawner.newBlock.transform.position.y + 5f;
        }
        else if (spawner.previousBlock != null)
        {
            targetY = spawner.previousBlock.transform.position.y + 5f;
        }
        else if (spawner.newBlock != null && spawner.previousBlock != null)
        {
            targetY = 8;
        }*/
        targetY = -0.83f;


        Vector3 targetPosition = new Vector3(
            0.24f,
            0.25f,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}