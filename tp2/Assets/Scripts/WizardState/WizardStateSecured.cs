using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateSecured : WizardState
{
    private int bunkeredHealthRegenRatio = 3;
    void Start()
    {

    }

    public override void Init()
    {
        // A wizard does not spawn as secured
    }

    public override void Attack()
    {
        //A bunkered opponent does not attack
    }

    public override void ManageStateChange()
    {
        if (manager.getNbLives() >= WizardManager.maxNbLives)
        {
            Debug.Log("Un-Bunkered");
            manager.ChangeState(WizardManager.WizardStateToSwitch.Normal);
        }
        else if (alerted)
        {
            alerted = false;
            //manager.ChangeState(WizardManager.WizardStateToSwitch.LastStand);
        }
    }

    public override void MoveWizard()
    {
        //A bunkered opponent does not move.
    }

    public override void Regenerate()
    {
        regenCadenceTimer += Time.deltaTime;
        if (manager.getNbLives() < WizardManager.maxNbLives)
        {

            if (regenCadenceTimer >= regenCadance)
            {
                regenCadenceTimer = 0;
                manager.AddRegenLives(bunkeredHealthRegenRatio * regen);
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
        // Despite not attacking, keep tracks of current targets as it will get out at some point
        if (target == null || target.tag.EndsWith("Tower"))
        {
            target = enemy.gameObject;
        }
    }

    public override void ManageEnemyExit(GameObject enemy)
    {
        // Despite not attacking, keep tracks of current targets as it will get out at some point
        if (target == null || enemy.gameObject == target)
        {
            isAttacking = false;
            SearchNewTarget();
        }
    }

    public override void ManageHidingSpotEnter(GameObject gameObject)
    {
        // Already hidden
    }

    public override void ManageIsAttackBy(GameObject gameObject)
    {
        //throw new System.NotImplementedException();
    }

    public void Alert()
    {
        alerted = true;
    }
}
