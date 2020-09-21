using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshKnight : Entity
{
    public string walkWpnAnimName = "walk_wpn";
    public string idleWpnAnimName = "idle_wpn";
    //public List<string> attackAnimNames = new List<string>(){ "attack", "attack_alt", "attack_alt_alt" };
    //public string attackAnimName = "attack";
    //public string attackAlternativeAnimName = "attack_alt";
    bool swordDrawn = true;
    public int reflexes = 2;
    public Transform combatTarget;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        stateLoop();
    }

    public void setIdle()
    {
        state = (int)states.idle;
        navAgent.ResetPath();
        if (swordDrawn)
        {
            animator.Play(idleWpnAnimName, 0);
        }
        else
        {
            animator.Play(idleAnimName, 0);
        }
    }

    protected override void stateLoop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            //Debug.Log("zeroing Vel");
        }

        if (state != (int)states.dead)
        {
            if (state == (int)states.idle)
            {

            }
            else if (state == (int)states.moveToTarget)
            {
                //checkArrival();
            }
            else if (state == (int)states.moveToTarget)
            {
                //checkArrival();
            }
            else if (state == (int)states.combat)
            {
                combatLoop();
            }
            else if (state == (int)states.wander)
            {
                //checkArrival();
            }
        }
        else if(state == (int)states.dead)
        {
            playDeathAnimation();
        }
    }



    void combatLoop()
    {
        /*if(combatTarget != null)
        {
            lookAtTarget();
            followCombatTarget();
            checkCombatRange();
        }*/
    }

    void checkCombatRange()
    {
        /*if(Vector3.Distance(transform.position, combatTarget.position) < weapon.range && !animatorIsInCombatTransition() && !animatorIsInStaggerTransition())
        {
            navAgent.ResetPath();
            int roll = Random.Range(0, 2);
            if(roll == 0)
            {
                if(!animatorIsInBlockTransition())
                {
                    weapon.attack();
                    playRandomAttackAnimation();
                }
            }
            else
            {
                animator.Play(blockAnimName, 0);
            }
        }
        else
        {
            if(!animatorIsInCombatTransition() && !animatorIsInStaggerTransition())
            {
                //Debug.Log("playin");
                animator.Play(walkWpnAnimName);
            }
        }*/
    }

    void followCombatTarget()
    {
        /*if (combatTarget != null)
        {
            if (!animatorIsInCombatTransition() && !animatorIsInStaggerTransition())
            {
                navAgent.SetDestination(combatTarget.position);
            }
        }*/
    }

    public void startCombat(Transform target)
    {
        animator.Play(walkWpnAnimName, 0);
    }

    void lookAtTarget()
    {
        //Vector3 direction = (combatTarget.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * reflexes);
    }

    public override void playRandomAttackAnimation()
    {
        if(!animatorIsInCombatTransition())
        {
            int random = Random.Range(0, attackAnimNames.Count);
            animator.Play(attackAnimNames[random], 0);
        }
    }

    public void setWanderDestination(float wanderRange)
    {
        state = (int)states.wander;
        animator.Play(walkWpnAnimName);
        Vector3 randomDirection = Random.insideUnitSphere * wanderRange;
        randomDirection += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, wanderRange, 1);
        Vector3 wanderPosition = navMeshHit.position;
        navAgent.destination = wanderPosition;
    }
}
