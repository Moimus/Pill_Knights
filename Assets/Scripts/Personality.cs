using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personality : MonoBehaviour
{
    /// <summary>
    /// retarded = 0,coward = 1,peaceful = 2,defensive = 3,aggresive = 4
    /// </summary>
    public enum traits
    {
        retarded = 0,
        coward = 1,
        peaceful = 2,
        defensive = 3,
        aggresive = 4
    }

    /// <summary>
    /// scout = 0,guard = 1
    /// </summary>
    public enum roles
    {
        scout = 0,
        guard = 1
    }

    public int trait = 0;
    public int role = 0;
    public float wanderRange = 1f;
    public NavMeshKnight controller;
    public float attention = 10f;
    public List<Entity> sensedTargets = new List<Entity>();
    //public Entity currentTarget = null;
    public bool active = false;
    public float decisionDelay = 60f;
    public float decisionRandomization = 10f;
    public float scanDelay = 1f;
    public float scanRandomization = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lateInit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator lifeCycle()
    {
        while(controller.state != (int)NavMeshKnight.states.dead)
        {
            scanForTargets();

            if(trait == (int)traits.aggresive && controller.combatTarget == null)
            {
                foreach (Entity en in sensedTargets)
                {
                    if (en.faction != transform.GetComponent<Entity>().faction)
                    {
                        GetComponent<NavMeshKnight>().startCombat(en.transform);
                        StopCoroutine(wanderCycle());
                        break;
                    }
                }
            }
            else if(trait == (int)traits.aggresive && controller.combatTarget != null)
            {
                if(controller.combatTarget.GetComponent<NavMeshKnight>() != null && controller.combatTarget.GetComponent<NavMeshKnight>().state == (int)NavMeshKnight.states.dead)
                {
                    controller.combatTarget = null;
                    StartCoroutine(wanderCycle());
                }
            }

            float rand = Random.Range(-scanRandomization, scanRandomization);
            yield return new WaitForSeconds(scanDelay);
            yield return null;
        }
        yield return null;
    }

    void init()
    {

    }

    IEnumerator lateInit()
    {
        yield return new WaitForSeconds(1.0f);
        if (role == (int)roles.scout)
        {
            Debug.Log("ScoutSet...");
            StartCoroutine(patrolCycle());

        }
        else if (role == (int)roles.guard)
        {
            Debug.Log("GuardSet...");
            StartCoroutine(wanderCycle());
        }
        //StartCoroutine(wanderCycle());
        StartCoroutine(lifeCycle());
        yield return null;
    }

    void roleRoutine()
    {
        if(role == (int)roles.scout)
        {

        }
        else if(role == (int)roles.guard)
        {

        }
    }

    protected void scanForTargets()
    {
        sensedTargets = null;
        sensedTargets = new List<Entity>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attention);
        foreach (Collider n in hitColliders)
        {
            if (n.transform.GetComponent<Entity>() != null)
            {
                if(n.transform.GetInstanceID() != transform.GetInstanceID())
                {
                    sensedTargets.Add(n.transform.GetComponent<Entity>());
                }
            }
        }
    }

    protected IEnumerator wanderCycle()
    {
        while (active)
        {

            if(controller.combatTarget == null)
            {
                controller.setWanderDestination(wanderRange);
            }
            float rand = Random.Range(-decisionRandomization, decisionRandomization);
            yield return new WaitForSeconds(decisionDelay + rand);
            yield return null;
        }
        yield return null;
    }

    protected IEnumerator patrolCycle()
    {
        /*controller.startPatrol(controller.parentChunk.waypoints);
        while (active)
        {
            float rand = Random.Range(-decisionRandomization, decisionRandomization);
            yield return new WaitForSeconds(decisionDelay + rand);
            yield return null;
        }*/
        yield return null;
    }
}
