using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateIntrepid : WizardState
{
    void Start()
    {
        speed = 2.5f;
        regen = normalRegen;
    }

    public override void Init()
    {
        target = manager.GetClosestEnemyTower();
    }

    public override void Attack()
    {
        if (target == null || !target.activeSelf)
            target = manager.GetClosestEnemyTower();

        isAttacking = Vector2.Distance(transform.position, target.transform.position) < range;

        if (isAttacking && canShoot)
        {
            manager.Attack(transform, target);
            HasShot();
        }
    }

    public override void ManageStateChange()
    {
        //pas de changement de state quand intrepid
    }

    public override void MoveWizard()
    {
        if (target != null && !isAttacking)
        {
            if (manager.IsInBush())
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime * WizardManager.bushReduction);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
        }
    }

    public override void Regenerate()
    {
        if (manager.getNbLives() < WizardManager.maxNbLives)
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
    public override void ManageEnemyEnter(GameObject gameObject)
    {
        //Doesnt interact with ennemy
    }

    public override void ManageEnemyExit(GameObject gameObject)
    {
        //Doesnt interact with ennemy
    }

    public override void ManageHidingSpotEnter(GameObject gameObject)
    {
        //Doesnt interact with new hiding spot
    }

    public override void ManageIsAttackBy(GameObject gameObject)
    {
        if(target.gameObject.tag.EndsWith("Tower"))
        {
            target = gameObject;
        }
    }
}