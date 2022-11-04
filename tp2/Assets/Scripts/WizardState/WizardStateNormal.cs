using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateNormal : WizardState
{
    void Start()
    {
        speed = 2f;
        targetTransform = manager.GetRandomActiveEnemyTower();
        targetIsTower = true;
    }

    public override void MoveWizard()
    {
        if (targetTransform != null && !isAttacking)
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
    }

    public override void ManageStateChange()
    {
        //if (Vector2.Distance(transform.position, target.position) < range)
        isAttacking = Vector2.Distance(transform.position, targetTransform.position) < range;

        if(isAttacking && canShoot)
        {
            lineController.DrawLine(transform, targetTransform);
            HasShot();
        }
    }
}
