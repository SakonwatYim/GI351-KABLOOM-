using UnityEngine;

public class camera : MonoBehaviour
{

    public blockMeneger spawner;

    public float speed = 5f;
    public float targetY;
    public float targetY1;

    void LateUpdate()
    {
        
            targetY = spawner.newBlock.transform.position.y;
        

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            targetY1,
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
