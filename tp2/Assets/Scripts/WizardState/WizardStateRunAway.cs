using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateRunAway : WizardState
{
    private bool isHiding;
    private float towerTransformX;

    void Start()
    {
        isHiding = false;
        regen = normalRegen;
        regenCadenceTimer = 0f;
        speed = 3f;

        GameObject closestTower = manager.GetClosestTower();
        towerTransformX = closestTower.transform.position.x;
        if (!manager.IsInBush())
        {
            List<GameObject> possibleSpot = manager.GetPossibleHidingSpots();
            if(possibleSpot.Count > 0)
            {
                PickClosestPoint(possibleSpot);
            }
            else
            {
                target = closestTower;
            }
        }
        else
        {
            target = manager.GetBush();
            isHiding = true;
        }
    }

    private void PickClosestPoint(List<GameObject> possibleSpot)
    {
        //a faire
        /*
         * if (Vector2.Distance(spot.transform.position, transform.position) < Vector2.Distance(target.transform.position, transform.position))
        {
            if((towerTransformX < transform.position.x && spot.transform.position.x < transform.position.x) ||
            (towerTransformX > transform.position.x && spot.transform.position.x > transform.position.x))
                target = spot;
        }*/
}

public override void Attack()
{
}

public override void ManageStateChange()
{
if(isHiding)
{
    if(target.tag == "Forest")
    {

    }
    else if(target.tag.EndsWith("Tower"))
    {

    }
}
}

public override void MoveWizard()
{
if(!isHiding && target != null)
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

}

// Reaction
public override void ManageEnemyEnter(GameObject gameObject)
{
}

public override void ManageEnemyExit(GameObject gameObject)
{
}

public override void ManageHidingSpotEnter(GameObject spot)
{
if (Vector2.Distance(spot.transform.position, transform.position) < Vector2.Distance(target.transform.position, transform.position))
{
    if((towerTransformX < transform.position.x && spot.transform.position.x < transform.position.x) ||
    (towerTransformX > transform.position.x && spot.transform.position.x > transform.position.x))
        target = spot;
}
}
}
