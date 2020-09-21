using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAggressiveMelee : MonoBehaviour, IAIPackage
{
    public AIController controller;
    public CreatureController[] sensedTargets;
    public CreatureController currentTarget = null;

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
        //throw new System.NotImplementedException();
    }

    public void run()
    {
        if(currentTarget == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, controller.creature.aggroRange);
            foreach(Collider n in hitColliders)
            {
                if(n.transform.GetComponent<CreatureController>() != null)
                {
                    currentTarget = n.GetComponent<CreatureController>();
                }
            }
        }
        else if(currentTarget != null)
        {
            if(Vector3.Distance(transform.position, currentTarget.transform.position) < controller.creature.attackRange)
            {
                if(!controller.creature.attacking)
                {
                    StartCoroutine(controller.creature.attackMelee());
                }
            }
            else
            {
                if(!controller.creature.attacking)
                {
                    controller.creature.lookAt(currentTarget.transform.position);
                    controller.creature.speedUp();
                    controller.creature.playAnimation("walk");
                }
            }

        }
    }
}
