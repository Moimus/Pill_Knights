using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsLiteCharacter : Entity
{
    public SoulsLiteController controller = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lateInit());
    }

    // Update is called once per frame
    void Update()
    {
        stateLoop();
        animationStateLoop();
    }

    public override IEnumerator lateInit()
    {
        yield return new WaitForSeconds(0.1f);

        yield return null;
    }

    public override void stagger()
    {
        SetStagger();
    }

    protected override void onHit()
    {
        checkAlive();
    }

    protected override void checkAlive()
    {
        if (hpCurrent <= 0)
        {
            onDeath();
            Destroy(gameObject, deathDelay);
        }
    }

    public override void onDeath() //TODO
    {
        controller.camRotator.parent = null;
        controller.camRotator = null;
        animator.speed = 0;
        List<Transform> transforms = new List<Transform>();
        getAllChildTransformsRecursive(transform, transforms);
        foreach (Transform t in transforms)
        {
            applyDeathShader(t);
        }
        state = (int)states.dead;
        animationState = (int)animationStates.dead;
    }
}
