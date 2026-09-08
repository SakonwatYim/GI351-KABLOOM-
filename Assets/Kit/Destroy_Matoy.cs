using UnityEngine;
using System.Collections;

public class Destroy_Matoy : MonoBehaviour
{
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
    void Start()
    {
       StartCoroutine(Wait());  
    }

    
}
