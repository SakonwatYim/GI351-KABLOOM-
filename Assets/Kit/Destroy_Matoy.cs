using UnityEngine;
using System.Collections;

public class Destroy_Matoy : MonoBehaviour
{
    blockMeneger meneger;
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(8f);
        meneger.matoy_count = 0;
        Destroy(gameObject);
    }
    void Start()
    {
        meneger = FindAnyObjectByType<blockMeneger>();
        StartCoroutine(Wait());  
    }

    
}
