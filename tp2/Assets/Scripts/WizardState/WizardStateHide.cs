using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateHide : WizardState
{

    private int exitBushFightingHpLimit = 65;
    private int hiddenHealthRegenRatio = 2;
    void Start()
    {
        
        regen = normalRegen;
        regenCadenceTimer = 0f;
        speed = 0f;
    }

    public override void Init()
    {
        //  A wizard does not get initialized as hidden.
    }

    public override void Attack()
    {
        if (target == null || !target.activeSelf)
            SearchNewTarget();


        if (target != null)
        {
            isAttacking = Vector2.Distance(transform.position, target.transform.position) < range;
        }
        else
        {
            isAttacking = false;
        }
            

        if (isAttacking && canShoot)
        {
            manager.Attack(transform, target);
            HasShot();
        }
    }

    public override void ManageStateChange()
    {
        if (manager.getNbLives() > exitBushFightingHpLimit && isAttacking)
        {
            manager.ChangeState(WizardManager.WizardStateToSwitch.Normal);
            target = manager.GetRandomActiveEnemyTower();
        }
        else if(manager.getNbLives() >= WizardManager.maxNbLives)
        {
            manager.ChangeState(WizardManager.WizardStateToSwitch.Normal);
        }
    }

    public override void MoveWizard()
    {
        // Hidden wizards do not move.
    }

    public override void Regenerate()
    {
        regenCadenceTimer += Time.deltaTime;
        if (!isAttacking && manager.getNbLives() < WizardManager.maxNbLives)
        {
            
            if (regenCadenceTimer >= regenCadance)
            {
                regenCadenceTimer = 0;
                manager.AddRegenLives(hiddenHealthRegenRatio * regen);
            }
        }
        else if(manager.getNbLives() < WizardManager.maxNbLives)
        {
            regenCadenceTimer = 0;
            manager.AddRegenLives(regen);
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
        // The character is already hidden
    }

    public override void ManageIsAttackBy(GameObject gameObject)
    {
        //throw new System.NotImplementedException();
    }
}