using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateNormal : WizardState
{
    void Start()
    {
        speed = 2f;
        towerPosition = manager.GetRandomActiveTower();
    }

    public override void MoveWizard()
    {
        if (towerPosition != null && !isAttacking)
            transform.position = Vector3.MoveTowards(transform.position, towerPosition.position, speed * Time.deltaTime);
    }

    public override void ManageStateChange()
    {
        if (!isAttacking && Vector2.Distance(transform.position, towerPosition.position) < range)
            isAttacking = true;

        if(isAttacking && canShoot)
        {
            lineController.DrawLine(transform, towerPosition);
            HasShot();
        }
    }
}
