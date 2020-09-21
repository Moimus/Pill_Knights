using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int resourceID = -1;
    public string resourceName = "name";

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
        if(resourceID == -1)
        {
            Debug.LogError("please assign an ID to this resource (index at ResourceManager.resourcePrefabs)");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Inventory>().add(resourceID);
            Destroy(gameObject);
        }
    }

    private void onColliderEnter(Collider other)
    {

    }
}
