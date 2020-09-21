using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoulsLiteAIController : Entity
{
    public Chunk parentChunk = null;
    public List<Entity> sensedEntities = new List<Entity>();
    public Entity combatTarget = null;
    public float wanderRadius = 10f;
    public float perceptionRange = 10f;
    public float reflexes = 2f;
    public float combatActionDelay = 0.5f;
    public float combatActionDelayRemaining = 0.0f;
    float scanInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        navAgent.ResetPath();
        StartCoroutine(scanCycle());
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
        equipRandomWeapon();
        yield return null;
    }

    protected override void stateLoop()
    {
        if (state != (int)states.dead)
        {
            if(state == (int)states.idle)
            {
                if(!animatorIsInIdleTransition() && !animatorIsInStaggerTransition() && !animatorIsInCombatTransition() && !animatorIsInRunAttackTransition())
                {
                    SetIdle();
                }
            }
            else if (state == (int)states.combat)
            {
                combatLoop();
            }
            else if (state == (int)states.wander)
            {
                wanderLoop();
            }
        }
        else
        {
            playDeathAnimation();
        }

        if (animatorIsInBlockTransition())
        {
            invincible = true;
        }
        else
        {
            invincible = false;
        }
    }

    protected void wanderLoop()
    {
        if(state == (int)states.wander && navAgent.hasPath == false)
        {
            setRandomWanderTarget();
            SetWalkForward();
        }
        else if(state == (int)states.wander && navAgent.hasPath != true)
        {
            if(arrivedAtTarget())
            {
                navAgent.ResetPath();
                SetIdle();
            }
        }
    }

    void setRandomWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, wanderRadius, 1);
        Vector3 wanderPosition = navMeshHit.position;
        navAgent.destination = wanderPosition;
    }

    protected void combatLoop()
    {
        if(combatTarget != null)
        {
            run();
            if (!animatorIsInCombatTransition() && !animatorIsInStaggerTransition())
            {
                lookAtTarget(); 
            }

            if (Vector3.Distance(transform.position, combatTarget.transform.position) < weapon.range)
            {
                navAgent.ResetPath();
                if(combatActionDelayRemaining <= 0)
                {
                    selectCombatAction();
                    combatActionDelayRemaining = combatActionDelay;
                }
                else
                {
                    if(!animatorIsInCombatTransition())
                    {
                        combatActionDelayRemaining -= Time.deltaTime;
                        SetIdle();
                    }
                }
            }
            else
            {
                if(!animatorIsInCombatTransition() && !animatorIsInStaggerTransition() && !animatorIsInBlockTransition())
                {
                    navAgent.destination = combatTarget.transform.position;
                }
            }
        }
        else
        {
            SetIdle();
            state = (int)states.idle;
        }
    }

    void lookAtTarget()
    {
        Vector3 direction = (combatTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * reflexes);
    }

    IEnumerator scanCycle()
    {
        while(state != (int)states.dead)
        {
            scanForTargets();
            selectCombatTarget();
            yield return new WaitForSeconds(scanInterval);
            yield return null;
        }
        yield return null;
    }

    void scanForTargets()
    {
        sensedEntities = new List<Entity>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, perceptionRange);
        foreach(Collider c in hitColliders)
        {
            if(c.transform.GetComponent<Entity>() != null && c.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                sensedEntities.Add(c.transform.GetComponent<Entity>());
            }
        }
    }

    void selectCombatTarget()
    {
        foreach(Entity e in sensedEntities)
        {
            if(e.faction != faction)
            {
                state = (int)states.combat;
                combatTarget = e;
                break;
            }
        }
    }

    void selectCombatAction()
    {
        if (!combatTarget.animatorIsInBlockTransition() && !combatTarget.animatorIsInCombatTransition())
        {
            SetAttacking();
        }
        else
        {
            setBlocking();
        }
    }

    bool arrivedAtTarget()
    {
        bool arrived = false;
        if(Math.valueIsBetween(Vector3.Distance(transform.position, navAgent.destination), -0.1f, 0.1f))
        {
            arrived = true;
        }

        return arrived;
    }

    protected override void moveForward()
    {
        SetWalkForward();
        navAgent.speed = speed;
    }

    protected override void moveBackWard()
    {
        navAgent.speed = speed;
    }

    protected override void run()
    {
        SetRun();
        navAgent.speed = speed * 1.7f;
    }

    public override void stagger()
    {
        base.stagger();
        combatActionDelayRemaining = combatActionDelay;
    }
}
