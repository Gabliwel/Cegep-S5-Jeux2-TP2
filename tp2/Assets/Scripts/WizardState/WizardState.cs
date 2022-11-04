using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    protected WizardManager manager;
    protected LineController lineController;

    protected GameObject target;
    protected Transform targetTransform;
    protected bool targetIsTower;

    protected const float range = 1.8f;
    protected bool isAttacking = false;
    protected float speed;
    protected bool canShoot = true;

    private const float cadence = 0.75f;
    private float cadenceTimer = 0;

    void Awake()
    {
        manager = GetComponent<WizardManager>();
        lineController = GetComponentInChildren<LineController>();
    }

    void Update()
    {
        MoveWizard();
        ManageStateChange();
        CheckCanShoot();
    }

    private void CheckCanShoot()
    {
        if(!canShoot)
        {
            cadenceTimer += Time.deltaTime;
            if (cadenceTimer >= cadence)
                canShoot = true;
        }
    }

    public void HasShot()
    {
        cadenceTimer = 0;
        canShoot = false;
    }

    public abstract void MoveWizard();
    public abstract void ManageStateChange();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled) return;

        if(targetIsTower && collision.gameObject.tag.EndsWith("Wizard") && gameObject.tag != collision.gameObject.tag)
        {
            targetIsTower = false;
            targetTransform = collision.gameObject.transform;
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
