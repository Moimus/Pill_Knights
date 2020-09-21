using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreasureSpawner : MonoBehaviour
{
    public bool looping = false;
    public float loopDelay = 60;
    public GameObject treasure;
    public string treasurePrefabFolder = "Prefabs/RNGTreasure";

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        GameObject[] treasureList = Resources.LoadAll<GameObject>(treasurePrefabFolder);
        int randomRoll = Random.Range(-(treasureList.Length - 1), treasureList.Length - 1);
        if(randomRoll >= 0)
        {
            treasure = treasureList[randomRoll];
            spawnOnce();
        }
    }

    public void spawnOnce()
    {
        GameObject treasureInstance = Instantiate(treasure, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
