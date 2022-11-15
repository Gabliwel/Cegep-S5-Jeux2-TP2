using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateNormal : WizardState
{
    void Start()
    {
        Init();
        speed = 1f;
        regen = normalRegen;
    }

    public override void Init()
    {
        target = manager.GetRandomActiveEnemyTower();
    }

    public override void MoveWizard()
    {
        if (target != null && !isAttacking)
        {
            if(manager.IsInBush())
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime * WizardManager.bushReduction);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
        }
    }

    public override void Attack()
    {
        if (target == null || !target.activeSelf)
        {
            SearchNewTarget();
            isAttacking = false;
            
        }
        else if (target.tag.EndsWith("Wizard"))
        {
            if(target.GetComponent<WizardManager>().GetWizardState() == WizardManager.WizardStateToSwitch.Secured)
            {
                SearchNewTarget();
                isAttacking = false;
            }
        }

        isAttacking = Vector2.Distance(transform.position, target.transform.position) < range;

        if (isAttacking && canShoot)
        {
            manager.Attack(transform, target);
            HasShot();
        }
    }

    public override void ManageStateChange()
    {
        if(manager.getNbLives() < WizardManager.maxNbLives / 4)
        {
            manager.ChangeState(WizardManager.WizardStateToSwitch.RunAway);
        } 
        else if(manager.GetNbKill() >= 3)
        {
            manager.ChangeState(WizardManager.WizardStateToSwitch.Intrepid);
        }
    }

    public override void Regenerate()
    {
        if (!isAttacking && manager.getNbLives() < WizardManager.maxNbLives)
        {
            regenCadenceTimer += Time.deltaTime;
            if (regenCadenceTimer >= regenCadance)
            {
                regenCadenceTimer = 0;
                manager.AddRegenLives(regen);
            }
        }
        else
        {
            regenCadenceTimer = 0;
        }
    }

    // Reaction
    public override void ManageEnemyEnter(GameObject enemy)
    {
        if (target == null || target.tag.EndsWith("Tower"))
        {
            target = enemy.gameObject;
        }
    }

    public override void ManageEnemyExit(GameObject enemy)
    {
        if (target == null || enemy.gameObject == target)
        {
            isAttacking = false;
            SearchNewTarget();
        }
    }

    public override void ManageHidingSpotEnter(GameObject gameObject)
    {
        //Doesnt interact with hiding spot
    }

    public override void ManageIsAttackBy(GameObject gameObject)
    {
        //Doesnt interact with new attack
    }
}
