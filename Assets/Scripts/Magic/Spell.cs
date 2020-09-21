using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float lifeTime = -1;
    public string displayName = "spell";
    public int specialCurrencyCost = 1;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void init()
    {
        if(lifeTime != -1)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    public virtual void lifeCycle()
    {

    }
}
