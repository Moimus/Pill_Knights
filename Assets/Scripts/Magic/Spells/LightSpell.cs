using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpell : Spell
{
    float lifeTimeRemaining = -1;
    float startDelay = -1;
    float endDelay = -1;
    public Light lightSource;
    public float lightMaxBrightness = 40;
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        lifeCycle();
    }

    public override void init()
    {
        base.init();
        placeDown();
        lightSource.intensity = 0;
        lifeTimeRemaining = lifeTime;
        startDelay = lifeTime * 0.1f;
        endDelay = lifeTime * 0.2f;
    }

    public override void lifeCycle()
    {
        lifeTimeRemaining -= Time.deltaTime;
        if(lifeTimeRemaining > lifeTime - startDelay)
        {
            if(lightSource.intensity <= lightMaxBrightness)
            {
                lightSource.intensity += Time.deltaTime * 10;
            }
        }
        else if(lifeTimeRemaining < endDelay)
        {
            if(lightSource.intensity >= 0)
            {
                lightSource.intensity -= Time.deltaTime * 10;
            }
        }
    }

    void placeDown()
    {
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(transform.position, -transform.up, out raycastHit, 5, -1);
        if (hit)
        {
            transform.position = raycastHit.point;
        }
    }
}
