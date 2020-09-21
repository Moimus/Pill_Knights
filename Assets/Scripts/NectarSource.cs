using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NectarSource : MonoBehaviour
{
    public float lastHarvested = -1;
    public float harvestCooldown = 10f;
    public float harvestValue = 120f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.GetComponent<Inventory>();
        if (inv != null)
        {
            if(inv.needsFuel)
            {
                harvest(inv);
            }
        }
    }

    void harvest(Inventory inventory)
    {
        if(Time.realtimeSinceStartup - lastHarvested > harvestCooldown || lastHarvested == -1)
        {
            Debug.Log("harvest nectar");
            lastHarvested = Time.realtimeSinceStartup;
            inventory.addFuel(harvestValue);
        }
        else
        {
            Debug.Log("harvest cooldown");
        }
    }
}
