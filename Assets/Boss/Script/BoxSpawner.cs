using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject box_prefab;
    void Start()
    {
        SpawnBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnBox()
    {
        GameObject obj=Instantiate(box_prefab);
        Vector3 temp = transform.position;
        temp.z = 0f;
        obj.transform.position = temp;
    }
}
