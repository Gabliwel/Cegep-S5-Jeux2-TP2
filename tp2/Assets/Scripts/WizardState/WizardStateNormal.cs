using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateNormal : WizardState
{
    void Start()
    {
        speed = 2f;
        regen = 5;
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
        if (!target.activeSelf)
        {
            SearchNewTarget();
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
    }

    public override void Regenerate()
    {
        if (!isAttacking && manager.getNbLives() < WizardManager.maxNbLives)
        {
            regenCadenceTimer += Time.deltaTime;
            if (regenCadenceTimer >= regenCadance)
                canShoot = true;
        }
        else
        {
            regenCadenceTimer = 0;
        }
    }
}
