using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    protected WizardManager manager;

    protected GameObject target;
    protected List<GameObject> possibleTargets = new();

    protected const float range = 1.8f;
    protected bool isAttacking = false;
    protected float speed;

    protected float regen;
    protected const float regenCadance = 1;
    protected float regenCadenceTimer = 0;

    private const float cadenceShoot = 0.5f;
    private float cadenceTimerShoot = 0;
    protected bool canShoot = true;


    void Awake()
    {
        manager = GetComponent<WizardManager>();
        target = manager.GetRandomActiveEnemyTower();
    }

    void Update()
    {
        MoveWizard();
        Attack();
        ManageStateChange();
        Regenerate();
        CheckCanShoot();
    }

    private void CheckCanShoot()
    {
        if(!canShoot)
        {
            cadenceTimerShoot += Time.deltaTime;
            if (cadenceTimerShoot >= cadenceShoot)
                canShoot = true;
        }
    }

    public void HasShot()
    {
        cadenceTimerShoot = 0;
        canShoot = false;
    }

    public abstract void MoveWizard();
    public abstract void Attack();
    public abstract void ManageStateChange();

    public abstract void Regenerate();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled) return;

        if (collision.gameObject.tag.EndsWith("Wizard") && gameObject.tag != collision.gameObject.tag && !collision.isTrigger)
        {
            possibleTargets.Add(collision.gameObject);
            Debug.Log(collision);

            if (target.tag.EndsWith("Tower"))
            {
                target = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!enabled) return;

        if (collision.gameObject.tag.EndsWith("Wizard") && gameObject.tag != collision.gameObject.tag && !collision.isTrigger)
        {
            possibleTargets.Remove(collision.gameObject);
            Debug.Log(collision);

            if (collision.gameObject == target)
            {
                isAttacking = false;
                SearchNewTarget();
            }
        }
    }

    private void SearchNewTarget()
    {
        if(possibleTargets.Count <= 0)
        {
            target = manager.GetRandomActiveEnemyTower();
        }
        else
        {
            GameObject closestTraget = null;
            float smallerDistance = Mathf.Infinity;

            foreach (GameObject possibleTarget in possibleTargets)
            {
                float distance = Vector2.Distance(transform.position, possibleTarget.transform.position);

                if (distance < smallerDistance)
                {
                    smallerDistance = distance;
                    closestTraget = possibleTarget;
                }
            }

            target = closestTraget;
        }
    }
}
