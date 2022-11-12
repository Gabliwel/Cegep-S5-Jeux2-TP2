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
        Init();
        isHiding = false;
        regen = normalRegen;
        regenCadenceTimer = 0f;
        speed = 3f;
    }

    public override void Init()
    {
        isHiding = false;
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

    private void PickClosestPoint(List<GameObject> possibleSpots)
    {
        GameObject closestSpot = null;
        float smallerDistance = Mathf.Infinity;

        foreach (GameObject possibleSpot in possibleSpots)
        {
            if ((towerTransformX < transform.position.x && possibleSpot.transform.position.x < transform.position.x) ||
                (towerTransformX > transform.position.x && possibleSpot.transform.position.x > transform.position.x))
            {
                float distance = Vector2.Distance(transform.position, possibleSpot.transform.position);

                if (distance < smallerDistance)
                {
                    smallerDistance = distance;
                    closestSpot = possibleSpot;
                }
            }
        }
        target = closestSpot;
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
                manager.ChangeState(WizardManager.WizardStateToSwitch.Hide);
            }
            else if(target.tag.EndsWith("Tower"))
            {
                manager.ChangeState(WizardManager.WizardStateToSwitch.Secured);
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

            if (target.transform.position == transform.position)
                isHiding = true;
        }
    }

    public override void Regenerate()
    {
        //Doesnt regenerate
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

    public override void ManageHidingSpotEnter(GameObject spot)
    {
        if (isHiding)
            return;

        if (Vector2.Distance(spot.transform.position, transform.position) < Vector2.Distance(target.transform.position, transform.position))
        {
            if((towerTransformX < transform.position.x && spot.transform.position.x < transform.position.x) ||
            (towerTransformX > transform.position.x && spot.transform.position.x > transform.position.x))
                target = spot;
        }
    }

    public override void ManageIsAttackBy(GameObject gameObject)
    {
        //Doesnt interact with new attack
    }
}
