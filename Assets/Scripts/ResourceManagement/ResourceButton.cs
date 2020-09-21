using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceButton : MonoBehaviour
{
    public int resourceID = -1;
    public Inventory inventory = null;
    public Text text;
    public Text countText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(int count)
    {
        if (resourceID == -1)
        {
            Debug.LogError("please assign an ID to this resource (index at ResourceManager.resourcePrefabs)");
        }
        text.text = ResourceManager.instance.resourcePrefabs[resourceID].GetComponent<Resource>().resourceName;
        countText.text = "x" + count.ToString();
    }

    public void drop()
    {
        inventory.drop(resourceID);
        Instantiate(ResourceManager.instance.resourcePrefabs[resourceID], inventory.dropMarker.position, inventory.transform.rotation);
        countText.text = "x" + inventory.resourceCount[resourceID].ToString();
        if(inventory.resourceCount[resourceID] <= 0)
        {
            inventory.resourceUI.destroyResourceButton(this);
        }
    }
}
