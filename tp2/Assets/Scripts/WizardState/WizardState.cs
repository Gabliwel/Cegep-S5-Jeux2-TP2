using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    protected WizardManager manager;

    protected GameObject target;

    protected const float range = 1.8f;
    protected bool isAttacking = false;
    protected float speed;

    protected float regen;
    protected const float regenCadance = 1;
    protected float regenCadenceTimer = 0;
    public const float normalRegen = 3f;

    protected float cadenceShoot = 0.40f;
    private float cadenceTimerShoot = 0;
    protected bool canShoot = true;
    protected bool alerted = false;


    void Awake()
    {
        manager = GetComponent<WizardManager>();
    }

    void Update()
    {
        MoveWizard();

        CheckCanShoot();
        Attack();

        ManageStateChange();
        Regenerate();
    }

    // Boucle
    public abstract void Init();
    public abstract void MoveWizard();
    public abstract void Attack();
    public abstract void ManageStateChange();
    public abstract void Regenerate();
    private void CheckCanShoot()
    {
        if (!canShoot)
        {
            cadenceTimerShoot += Time.deltaTime;
            if (cadenceTimerShoot >= cadenceShoot)
                canShoot = true;
        }
    }

    // Reaction
    public abstract void ManageEnemyEnter(GameObject gameObject);
    public abstract void ManageEnemyExit(GameObject gameObject);
    public abstract void ManageHidingSpotEnter(GameObject gameObject);
    public abstract void ManageIsAttackBy(GameObject gameObject);

    // Autre
    public void SearchNewTarget()
    {
        if (manager.GetPossibleTargets().Count <= 0)
        {
            target = manager.GetRandomActiveEnemyTower();
        }
        else
        {
            GameObject closestTarget = null;
            float smallerDistance = Mathf.Infinity;

            foreach (GameObject possibleTarget in manager.GetPossibleTargets())
            {

                float distance = Vector2.Distance(transform.position, possibleTarget.transform.position);

                if (distance < smallerDistance)
                {
                    if (possibleTarget.tag.EndsWith("Wizard"))
                    {
                        if (possibleTarget.GetComponent<WizardManager>().GetWizardState() != WizardManager.WizardStateToSwitch.Secured)
                        {
                            smallerDistance = distance;
                            closestTarget = possibleTarget;
                        }
                    }
                    else
                    {
                        smallerDistance = distance;
                        closestTarget = possibleTarget;
                    }
                    
                }
            }
            if (closestTarget == null)
            {
                closestTarget = manager.GetClosestEnemyTower();
            }

            target = closestTarget;
        }
    }

    public void HasShot()
    {
        cadenceTimerShoot = 0;
        canShoot = false;
    }
    
    
}
