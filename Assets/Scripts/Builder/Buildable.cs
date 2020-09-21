using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate()
    {
        if(GetComponent<Collider>() != null && !GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = true;
        }
    }
}
