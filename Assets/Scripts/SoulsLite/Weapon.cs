using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 0.5f;
    public int damage = 1;
    public Entity owner = null;
    public float attackCooldown = 0.8f;
    float attackCooldownRemaining = 0f;
    public int staggerChance = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer();
    }

    public void attack()
    {
        if(attackCooldownRemaining <= 0)
        {
            attackCooldownRemaining = attackCooldown;
        }
    }

    void coolDownTimer()
    {
        if (attackCooldownRemaining > 0)
        {
            attackCooldownRemaining -= Time.deltaTime;
        }
        else
        {
            attackCooldownRemaining = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            if (owner.animatorIsInCombatTransition() || owner.animatorIsInRunAttackTransition())
            {
                if (other.GetComponent<Weapon>() != null)
                {
                    Weapon otherWeapon = other.GetComponent<Weapon>();
                    if (otherWeapon.owner.animatorIsInBlockTransition())
                    {
                        Debug.Log("Blocked!");
                        owner.stagger();
                    }
                }
                else if (owner.animatorIsInCombatTransition() && other.GetComponent<Entity>() != null && other.GetComponent<Entity>().faction != owner.faction)
                {
                    Entity entity = other.GetComponent<Entity>();
                    if(!entity.invincible)
                    {
                        entity.damage(damage);
                        entity.spawnOnHitFx(transform.position);
                        if (rollForStagger())
                        {
                            entity.stagger();
                        }
                    }
                }
                else if(owner.animatorIsInRunAttackTransition() && other.GetComponent<Entity>() != null && other.GetComponent<Entity>().faction != owner.faction)
                {
                    Entity entity = other.GetComponent<Entity>();
                    if (!entity.invincible)
                    {
                        entity.damage(damage);
                        entity.spawnOnHitFx(transform.position);
                        entity.stagger();
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    bool rollForStagger()
    {
        bool success = false;
        int random = UnityEngine.Random.Range(0, 100);
        if(staggerChance >= random)
        {
            success = true;
        }
        return success;
    }

}
