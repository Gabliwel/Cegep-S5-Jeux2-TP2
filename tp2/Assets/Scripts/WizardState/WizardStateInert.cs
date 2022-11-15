using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateInert : WizardState
{
    public override void Init()
    {
        //Inert
    }

    public override void Attack()
    {
        //Inert
    }

    public override void ManageStateChange()
    {
        //Inert
    }

    public override void MoveWizard()
    {
        //Inert
    }

    public override void Regenerate()
    {
        //Inert
    }

    // Reaction
    public override void ManageEnemyEnter(GameObject enemy)
    {
        ///Inert
    }

    public override void ManageEnemyExit(GameObject enemy)
    {
        //Inert
    }

    public override void ManageHidingSpotEnter(GameObject gameObject)
    {
        //Inert
    }

    public override void ManageIsAttackBy(GameObject gameObject)
    {
        //Inert
    }
}
