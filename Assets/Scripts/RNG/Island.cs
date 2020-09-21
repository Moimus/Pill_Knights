using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public static Island instance;
    public Transform[] pointsOfInterest;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform[] getPOIs()
    {
        return pointsOfInterest;
    }
}
