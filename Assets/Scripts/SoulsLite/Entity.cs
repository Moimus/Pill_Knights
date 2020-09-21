using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    //
    public enum animationStates
    {
        dead = -1,
        idle = 0,
        walkingForward = 1,
        walkingBackward = 2,
        running = 3,
        attacking = 4,
        blocking = 5,
        staggering = 6
    }
    public int animationState = 0;

    /// <summary>
    /// dead = -1, idle = 0, moveToTarget = 1, combat = 2, wander = 3
    /// </summary>
    public enum states
    {
        dead = -1,
        idle = 0,
        moveToTarget = 1,
        combat = 2,
        wander = 3
    }
    public int state = 0;

    public float speed = 1f;
    public Weapon weapon = null;
    public Transform weaponHand;
    public int faction = 0;
    public int hpMax = 10;
    public int hpCurrent = 10;
    public bool invincible = false;
    public float deathDelay = 0;
    public NavMeshAgent navAgent;
    public string walkAnimName = "walk_wpn";
    public string runAnimName = "run";
    public string runAttackAnimName = "run_attack";
    public string idleAnimName = "idle_wpn";
    public string blockAnimName = "block";
    public List<string> attackAnimNames = new List<string>() { "attack", "attack_alt", "attack_alt_alt" };
    public string staggerAnimationName = "stagger";
    public Animator animator;

    public GameObject onHitFx;
    //death stuff
    /// <summary>
    /// material from assets DO NOT MODIFY at runtime!
    /// </summary>
    public Material dissolveMaterialBlueprint = null;
    /// <summary>
    /// cloned material
    /// </summary>
    protected Material dissolveMaterialInstance = null;
    /// <summary>
    /// -1 Animation Start, 1 Animation End
    /// </summary>
    protected float dissolveAnimationProgress = -0.8f;

    // Start is called before the first frame update
    void Start()
    {
        equipRandomWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        stateLoop();
        animationStateLoop();
    }

    public virtual void init()
    {
        hpCurrent = hpMax;
    }

    public virtual IEnumerator lateInit()
    {
        yield return null;
    }

    protected virtual void stateLoop()
    { //TODO
        if (state != (int)states.dead)
        {

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

    protected virtual void animationStateLoop()
    {
        if (state != (int)states.dead)
        {
            if (animationState == (int)animationStates.idle)
            {
                if (!animatorIsInIdleTransition() && !animatorIsInCombatTransition() && !animatorIsInRunAttackTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition())
                {
                    animator.Play(idleAnimName, 0);
                }
            }
            else if (animationState == (int)animationStates.walkingForward)
            {
                if (!animatorIsInBlockTransition() && !animatorIsInCombatTransition())
                {
                    moveForward();
                    if (!animatorIsInWalkTransition())
                    {
                        animator.Play(walkAnimName, 0);
                    }
                }
            }
            else if (animationState == (int)animationStates.walkingBackward)
            {
                if (!animatorIsInBlockTransition() && !animatorIsInCombatTransition() && !animatorIsInStaggerTransition())
                {
                    moveBackWard();
                    if (!animatorIsInWalkTransition())
                    {
                        animator.Play(walkAnimName, 0);
                    }
                }
            }
            else if (animationState == (int)animationStates.running)
            {
                if (!animatorIsInBlockTransition() && !animatorIsInCombatTransition() && !animatorIsInStaggerTransition())
                {
                    run();
                    if (!animatorIsInRunTransition() && !animatorIsInRunAttackTransition())
                    {
                        animator.Play(runAnimName, 0);
                    }
                }
            }
            else if (animationState == (int)animationStates.attacking)
            {
                if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition())
                {
                    if (!animatorIsInRunTransition())
                    {
                        playRandomAttackAnimation();
                        weapon.attack();
                        animationState = (int)animationStates.idle;
                    }
                    else if (animatorIsInRunTransition())
                    {
                        animator.Play(runAttackAnimName, 0);
                        weapon.attack();
                        animationState = (int)animationStates.idle;
                    }
                }
            }
            else if (animationState == (int)animationStates.blocking)
            {
                if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition())
                {
                    animator.Play(blockAnimName, 0);
                    animationState = (int)animationStates.idle;
                }
            }
            else if (animationState == (int)animationStates.staggering)
            {
                if (!animatorIsInStaggerTransition())
                {
                    animator.Play(staggerAnimationName, 0);
                    animationState = (int)animationStates.idle;
                }
            }
        }
    }

    public void SetStagger()
    {
        if (!animatorIsInStaggerTransition())
        {
            animationState = (int)animationStates.staggering;
        }
    }

    public void SetAttacking()
    {
        animationState = (int)animationStates.attacking;
    }

    public void SetIdle()
    {
        if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition() && !animatorIsInRunAttackTransition())
        {
            if (animationState != (int)animationStates.attacking && animationState != (int)animationStates.blocking)
            {
                animationState = (int)animationStates.idle;
            }
        }
    }

    public void SetWalkForward()
    {
        if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition() && !animatorIsInRunAttackTransition())
        {
            if (animationState != (int)animationStates.attacking && animationState != (int)animationStates.blocking)
            {
                animationState = (int)animationStates.walkingForward;
            }
        }
    }

    public void SetRun()
    {
        if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition())
        {
            if (animationState != (int)animationStates.attacking && animationState != (int)animationStates.blocking)
            {
                animationState = (int)animationStates.running;
            }
        }
    }

    public void SetWalkBackward()
    {
        if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition() && !animatorIsInRunAttackTransition())
        {
            if (animationState != (int)animationStates.attacking && animationState != (int)animationStates.blocking)
            {
                animationState = (int)animationStates.walkingBackward;
            }
        }
    }

    public void setBlocking()
    {
        if (!animatorIsInCombatTransition() && !animatorIsInBlockTransition() && !animatorIsInStaggerTransition())
        {
            animationState = (int)animationStates.blocking;
        }
    }

    protected virtual void moveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void moveBackWard()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void run()
    {
        transform.Translate(Vector3.forward * speed * 1.7f * Time.deltaTime);
    }

    public void rotate(float scale)
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100 * scale);
    }

    public void attack()
    {
        if (weapon != null)
        {
            weapon.attack();
        }
    }

    //recursive helper function
    protected void getAllChildTransformsRecursive(Transform parent, List<Transform> output)
    {
        if (parent.childCount > 0)
        {
            foreach (Transform t in parent)
            {
                output.Add(t);
                getAllChildTransformsRecursive(t, output);
            }
        }
    }

    public virtual void stagger()
    {
        animator.Play(staggerAnimationName, 0);
        GetComponent<Rigidbody>().AddForce(-Vector3.forward * 2);
    }

    public void damage(int amount)
    {
        if(!invincible)
        {
            hpCurrent -= amount;
            onHit();
        }
    }

    protected virtual void onHit()
    {
        checkAlive();
    }

    public void spawnOnHitFx(Vector3 position)
    {
        if(onHitFx != null)
        {
            GameObject hitInstance = Instantiate(onHitFx, position, Quaternion.Euler(0,0,0));
        }
    }

    /// <summary>
    /// is the entitiy idling?
    /// </summary>
    /// <returns></returns>
    public bool animatorIsInIdleTransition()
    {
        bool val = false;
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == idleAnimName)
            {
                val = true;
            }
        }
        return val;
    }

    /// <summary>
    /// is the entitiy walking?
    /// </summary>
    /// <returns></returns>
    public bool animatorIsInWalkTransition()
    {
        bool val = false;
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(walkAnimName))
            {
                val = true;
            }
        }
        return val;
    }

    /// <summary>
    /// is the entitiy running?
    /// </summary>
    /// <returns></returns>
    public bool animatorIsInRunTransition()
    {
        bool val = false;
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(runAnimName))
            {
                val = true;
            }
        }
        return val;
    }

    /// <summary>
    /// is the entitiy blocking?
    /// </summary>
    /// <returns></returns>
    public bool animatorIsInBlockTransition()
    {
        bool val = false;
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(blockAnimName))
            {
                val = true;
            }
        }
        return val;
    }

    public bool animatorIsInStaggerTransition()
    {
        bool val = false;
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(staggerAnimationName))
            {
                val = true;
            }
        }
        return val;
    }

    /// <summary>
    /// is the entitiy attacking?
    /// </summary>
    /// <returns></returns>
    public bool animatorIsInCombatTransition()
    {
        bool val = false;
        foreach (string s in attackAnimNames)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(s))
            {
                val = true;
            }
        }
        return val;
    }

    public bool animatorIsInRunAttackTransition()
    {
        bool val = false;
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(runAttackAnimName))
            {
                val = true;
            }
        }
        return val;
    }


    //TODO
    protected virtual void checkAlive()
    {
        if (hpCurrent <= 0)
        {
            playDeathAnimation();
            onDeath();
            dropWeapon();
            Destroy(gameObject, deathDelay);
        }
    }

    public virtual void onDeath() //TODO
    {
        animator.speed = 0;
        navAgent.ResetPath();
        List<Transform> transforms = new List<Transform>();
        getAllChildTransformsRecursive(transform, transforms);
        foreach (Transform t in transforms)
        {
            applyDeathShader(t);
        }
        state = (int)states.dead;
        animationState = (int)animationStates.dead;
    }

    protected void applyDeathShader(Transform trans)
    {
        if (trans.GetComponent<Renderer>() != null && trans.GetComponent<ParticleSystem>() == null)
        {
            Debug.Log("changing Material");
            dissolveMaterialInstance = new Material(dissolveMaterialBlueprint);
            trans.GetComponent<Renderer>().materials = new Material[] { dissolveMaterialInstance };
        }
    }

    public virtual void playRandomAttackAnimation()
    {
        if (!animatorIsInCombatTransition())
        {
            int random = Random.Range(0, attackAnimNames.Count);
            animator.Play(attackAnimNames[random], 0);
        }
    }

    protected void playDeathAnimation()
    {
        List<Transform> transforms = new List<Transform>();
        getAllChildTransformsRecursive(transform, transforms);
        foreach (Transform t in transforms)
        {
            if (t.GetComponent<Renderer>() != null)
            {
                dissolveAnimationProgress += (0.1f * Time.deltaTime);
                t.GetComponent<Renderer>().material.SetFloat("AnimationProgress", dissolveAnimationProgress);
            }
        }
    }

    protected void zeroVelocity()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Debug.Log("zeroing Vel");
        }
    }

    public void equipRandomWeapon()
    {
        if(weapon != null)
        {
            Destroy(weapon.gameObject);
            weapon = null;
        }
        GameObject sword = Instantiate(ResourceManager.instance.procedualWeaponPrefab, weaponHand);
        ProcedualSword proSword = sword.GetComponent<ProcedualSword>();
        proSword.init(this);
        weapon = sword.GetComponent<Weapon>();
        sword.transform.position = weaponHand.position;
        sword.transform.rotation = weaponHand.rotation;
        sword.transform.localRotation = Quaternion.Euler(0, 90, -180);
        //TODO fix this
        sword.transform.localScale = new Vector3(sword.transform.localScale.x * 0.01f, sword.transform.localScale.y * 0.01f, sword.transform.localScale.z * 0.01f);
    }

    public void loadWeapon(ProcedualSwordModel model)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
            weapon = null;
        }
        GameObject sword = Instantiate(ResourceManager.instance.procedualWeaponPrefab, weaponHand);
        ProcedualSword proSword = sword.GetComponent<ProcedualSword>();
        proSword.initFromModel(this, model);
        weapon = sword.GetComponent<Weapon>();
        sword.transform.position = weaponHand.position;
        sword.transform.rotation = weaponHand.rotation;
        sword.transform.localRotation = Quaternion.Euler(0, 90, -180);
        //TODO fix this
        sword.transform.localScale = new Vector3(sword.transform.localScale.x * 0.01f, sword.transform.localScale.y * 0.01f, sword.transform.localScale.z * 0.01f);
    }

    public void dropWeapon()
    {
        if(weapon != null)
        {
            GameObject dropObject = Instantiate(ResourceManager.instance.weaponDropPrefab, transform.position, transform.rotation);
            dropObject.GetComponent<WeaponDrop>().init(weapon.GetComponent<ProcedualSword>().swordModel);
            weapon = null;
        }

    }
}
