using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMAtTest : MonoBehaviour
{
    public Material testMat = null;
    // Start is called before the first frame update
    void Start()
    {
        //switchMat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchMat()
    {
        Debug.Log("changing Mat");
        GetComponent<Renderer>().materials = new Material[]{testMat};
    }
}
