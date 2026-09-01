using UnityEngine;

public class camera : MonoBehaviour
{

    public blockMeneger spawner;

    public float speed = 5f;

    void LateUpdate()
    {
        

        float targetY = spawner.previousBlock.transform.position.y;

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            targetY,
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
